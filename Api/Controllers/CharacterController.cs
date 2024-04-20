using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence;
using Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Applicaton.Services.CharacterService;
using Applicaton.Dto.Character;
using Core.Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }



        [HttpGet, Route("")]
        public IActionResult GetAll([FromQuery(Name = "page")] int page, [FromQuery(Name = "name")] string? name, [FromQuery(Name = "status")] string? status, [FromQuery(Name = "species")] string? species, [FromQuery(Name = "type")] string? type, [FromQuery(Name = "gender")] string? gender)
        {
            var response = _characterService.GetCharacters(page, name, status, species, type, gender);
            return Ok(response);
        }

        [HttpGet, Route("{ids}")]
        public IActionResult GetCharactersById([FromRoute] string ids)
        {
            List<int> intIds = ids.Split(',').Select(int.Parse).ToList();
            var response = _characterService.GetCharactersById(intIds);
            if (response?.Data?.Count == 1)
                return Ok(Response<CharacterDto>.Success(200, response.Data.FirstOrDefault()));
            return Ok(response);
        }


    }
}