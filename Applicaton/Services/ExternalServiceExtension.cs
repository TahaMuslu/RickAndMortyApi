
using Applicaton.Services.CharacterService;
using Application.Services.TokenService;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public static class ExternalServiceExtension
    {
        public static void ExternalServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService.TokenService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<JwtGenerator, JwtGenerator>();
        }

    }
}