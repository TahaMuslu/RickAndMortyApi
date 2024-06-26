﻿using Applicaton.Settings;
using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Middleware
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        public CustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                UrlSettings.BaseUrl = httpContext.Request.Scheme + "://" + httpContext.Request.Host.Value;
                Log.Information($"Request {httpContext.Request.Path} is called ");
                await _next(httpContext);
                Log.Information($"Request {httpContext.Request.Method} {httpContext.Request.Path} => {httpContext.Response.StatusCode} is finished");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            Response<object> responseModel = Response<object>.Fail(500, "Internal Server Error");

            if (ex is UnauthorizedAccessException)
            {
                responseModel.Messages = new List<string> { "Unauthorized Access" };
                responseModel.StatusCode = 401;
                Log.Error(ex,$"Unauthorized Access: {ex.Message}  - - " +
                                 $"Request  {httpContext.Request.Method} {httpContext.Request.Path}");
            }
            else
            {
                Log.Error(ex,$"Internal Server Error: {ex.Message}  - - " +
                                 $"Request  {httpContext.Request.Method} {httpContext.Request.Path}");
            }

            httpContext.Response.StatusCode = responseModel.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(responseModel,typeof(Response<object>));
        }
    }
}
