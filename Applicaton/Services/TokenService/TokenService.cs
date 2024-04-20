using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public TokenService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public Guid GetUserId()
        {
            var identity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                string? id = userClaims.FirstOrDefault(o => o.Type == "UserId")?.Value;
                if (id != null)
                    return Guid.Parse(id);
            }

            throw new Exception("User not found");
        }
    }
}
