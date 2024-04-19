
using Apposite.Application.Services.CuisinePreferenceService;
using Apposite.Application.Services.HealthService;
using Apposite.Application.Services.IngredientService;
using Apposite.Application.Services.MailService;
using Apposite.Application.Services.MediaFile;
using Apposite.Application.Services.TokenService;
using Microsoft.Extensions.DependencyInjection;

namespace Apposite.Application.Services
{
    public static class ExternalServiceExtension
    {
        public static void ExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IIngredientService, IngredientService.IngredientService>();
            services.AddSingleton<ICuisinePreferenceService, CuisinePreferenceService.CuisinePreferenceService>();
            services.AddSingleton<IHealthService, HealthService.HealthService>();
            services.AddScoped<ITokenService, TokenService.TokenService>();
            services.AddScoped<IMailService, MailService.MailService>();
            services.AddSingleton<IMediaFileService, MediaFileService>();
        }

    }
}