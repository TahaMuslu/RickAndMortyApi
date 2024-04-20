using Persistence;
using Microsoft.AspNetCore.Mvc;
using Applicaton.Dto.Auth;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RickAndMortyDbContext _dbContext;
        private readonly JwtGenerator jwtGenerator;
        public AuthController(RickAndMortyDbContext dbContext, JwtGenerator jwtGenerator)
        {
            _dbContext = dbContext;
            this.jwtGenerator = jwtGenerator;
        }

        /// <summary>
        /// Login
        /// </summary>
        ///<remarks>
        ///Example Request:
        ///
        ///     Admin
        ///     {
        ///         "email":"admin@admin.com",
        ///         "password":"P@ssw0rd"
        ///     }
        ///  
        ///     
        /// </remarks>
        /// <returns>
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null)
                return StatusCode(401, Response<NoContent>.Fail(401, "Wrong Email or Password"));
            if (!CryptoService.VerifyPassword(loginDto.Password,user.Password))
                return StatusCode(401, Response<NoContent>.Fail(401, "Wrong Password"));

            string token = await jwtGenerator.GenerateJwt(user);

            return StatusCode(200, Response<object>.Success(200, "Login Success",new{ Token = token }));
        }



    }
}
