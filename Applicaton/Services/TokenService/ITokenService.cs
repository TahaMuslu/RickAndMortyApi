using Microsoft.AspNetCore.Identity;

namespace Apposite.Application.Services.TokenService
{
    public interface ITokenService
    {

        public Guid GetUserId();

    }
}