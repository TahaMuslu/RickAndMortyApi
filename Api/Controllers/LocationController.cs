using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence;
using Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Applicaton.Services.CharacterService;
using Applicaton.Services.LocationService;
using Applicaton.Dto.Character;
using Core.Dtos;
using Applicaton.Dto.Location;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }



        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllAsync([FromQuery(Name = "page")] int page, [FromQuery(Name = "name")] string? name, [FromQuery(Name = "type")] string? type, [FromQuery(Name = "dimension")] string? dimension)
        {
            var response = _locationService.GetLocations(page);
            return Ok(response);
        }


        [HttpGet, Route("{ids}")]
        public IActionResult GetLocationsById([FromRoute] string ids)
        {
            List<int> intIds = ids.Split(',').Select(int.Parse).ToList();
            var response = _locationService.GetLocationsById(intIds);
            if (response?.Data?.Count == 1)
                return Ok(Response<LocationDto>.Success(200, response.Data.FirstOrDefault()));
            return Ok(response);
        }


    }
}