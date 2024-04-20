using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence;
using Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Applicaton.Services.CharacterService;
using Applicaton.Services.EpisodeService;
using Applicaton.Dto.Location;
using Core.Dtos;
using Applicaton.Dto.Episode;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeService _episodeService;

        public EpisodeController(IEpisodeService episodeService)
        {
            _episodeService = episodeService;
        }



        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllAsync([FromQuery(Name = "page")] int page, [FromQuery(Name = "name")] string? name, [FromQuery(Name = "episode")] string? episode)
        {
            var response = _episodeService.GetEpisodes(page, name, episode);
            return Ok(response);
        }


        [HttpGet, Route("{ids}")]
        public IActionResult GetEpisodesById([FromRoute] string ids)
        {
            List<int> intIds = ids.Split(',').Select(int.Parse).ToList();
            var response = _episodeService.GetEpisodesById(intIds);
            if (response?.Data?.Count == 1)
                return Ok(Response<EpisodeDto>.Success(200, response.Data.FirstOrDefault()));
            return Ok(response);
        }


    }
}