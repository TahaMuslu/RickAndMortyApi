using Apposite.Application.Services;
using Apposite.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Apposite.Application.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<User> _userManager, JwtGenerator jwtGenerator)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validateTokenResult = await jwtGenerator.ValidateToken(token);
            if (validateTokenResult.IsValid)
            {
                context.Items["User"] = await _userManager.FindByIdAsync(validateTokenResult.UserId);
            }

            await _next(context);
        }
    }
}