
using Applicaton.Services.CharacterService;
using Application.Services.TokenService;
using Microsoft.Extensions.DependencyInjection;
using Applicaton.Services.LocationService;
using Applicaton.Services.EpisodeService;

namespace Application.Services
{
    public static class ExternalServiceExtension
    {
        public static void ExternalServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService.TokenService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<JwtGenerator, JwtGenerator>();
        }

    }
}