using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Apposite.Application.ServiceExtensions
{
    public static class LoggerExtensions
    {
        public static void ConfigureLogging()
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .Enrich.WithProperty("Environment", environment)
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

            }
            catch (Exception ex)
            {
                Log.Warning("Logger Exception: " + ex.Message);
            }
        }

        public static void SendError<T>(this ILogger<T> _logger, Exception ex, [CallerMemberName] string methodName = "")
        {
            _logger.LogError($"{methodName} throw an exception. Exception Message: {ex.Message}. Inner Exception Message: {(ex.InnerException != null ? ex.InnerException.Message : "")}");
        }
        public static void SendError<T>(this ILogger<T> _logger, string message, Exception ex, [CallerMemberName] string methodName = "")
        {
            _logger.LogError($"{methodName}: {message}. Exception Message: {ex.Message}. Inner Exception Message: {(ex.InnerException != null ? ex.InnerException.Message : "")}");
        }
        public static void SendWarning<T>(this ILogger<T> _logger, object o, [CallerMemberName] string methodName = "")
        {
            _logger.LogWarning($"{methodName} warning. {System.Text.Json.JsonSerializer.Serialize(o)}");
        }
        public static void SendWarning<T>(this ILogger<T> _logger, string message, [CallerMemberName] string methodName = "")
        {
            _logger.LogWarning($"{methodName} warning. {message}");
        }
        public static void SendInformation<T>(this ILogger<T> _logger, object o, [CallerMemberName] string methodName = "")
        {
            _logger.LogInformation($"{methodName} just started. Request: {System.Text.Json.JsonSerializer.Serialize(o)}");
        }
        public static void SendInformation<T>(this ILogger<T> _logger, string message, [CallerMemberName] string methodName = "")
        {
            _logger.LogInformation($"{methodName}. {message}");
        }
    }

}