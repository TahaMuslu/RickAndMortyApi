using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {

        public CharacterController()
        {
        }

        

        [HttpGet,Route("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }


    }
}