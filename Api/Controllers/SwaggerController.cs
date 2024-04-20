using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/")]
    public class SwaggerController : ControllerBase
    {

        [HttpGet, Route("")]
        public void Swagger()
        {
            Response.Redirect("/swagger/index.html");
        }


    }
}
