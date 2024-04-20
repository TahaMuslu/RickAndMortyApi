using Applicaton.Dto.Character;
using Applicaton.Dto.Episode;
using Applicaton.Dto.Location;
using Applicaton.Services;
using Applicaton.Settings;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Persistence.Mapper
{
    public class MappingProfile : Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public MappingProfile()
        {
            CreateMap<Character, CharacterDto>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlSettings.BaseUrl + "/api/character/" + src.Id))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => new CharacterOriginDto() { Name = src.Origin.Name, Url = UrlSettings.BaseUrl + "/api/location/" + src.Origin.Id }))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new CharacterLocationDto() { Name = src.Location.Name, Url = UrlSettings.BaseUrl + "/api/location/" + src.Location.Id }))
                .ForMember(dest => dest.Episode, opt => opt.MapFrom(src => src.Episodes.Select(e => UrlSettings.BaseUrl + "/api/episode/" + e.Id).ToList()));

            CreateMap<Episode,EpisodeDto>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                .ForMember(dest => dest.Episode, opt => opt.MapFrom(src => src.EpisodeCode))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlSettings.BaseUrl + "/api/episode/" + src.Id))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters.Select(c => UrlSettings.BaseUrl + "/api/character/" + c.Id).ToList()));

            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => UrlSettings.BaseUrl + "/api/location/" + src.Id))
                .ForMember(dest => dest.Residents, opt => opt.MapFrom(src => src.Residents.Select(c => UrlSettings.BaseUrl + "/api/character/" + c.Id).ToList()));


        }
    }

}
