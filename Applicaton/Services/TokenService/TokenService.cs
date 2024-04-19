using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Apposite.Application.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtSecurityTokenHandler _handler;


        public TokenService(ILogger<TokenService> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _handler = new JwtSecurityTokenHandler();
        }


        public Guid GetUserId()
        {
            var token = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token is empty");
                throw new Exception("Token is empty");
            }

            var jsonToken = _handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                _logger.LogError("Token is invalid");
                throw new Exception("Token is invalid");
            }

            var userId = jsonToken.Claims.First(claim => claim.Type == "id").Value;

            return new Guid(userId);
        }
    }
}
