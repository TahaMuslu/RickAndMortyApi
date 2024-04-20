using Applicaton.Dto.Episode;
using Core.Dtos;

namespace Applicaton.Services.EpisodeService
{
    public interface IEpisodeService
    {
        Response<List<EpisodeDto>> GetEpisodes(int page, string? name = null, string? episode = null);
        Response<List<EpisodeDto>> GetEpisodesById(List<int> ids);
    }
}
