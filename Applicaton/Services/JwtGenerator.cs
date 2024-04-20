using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Application.Settings;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.Services
{
    public class JwtGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly RickAndMortyDbContext _dbcontext;
        private readonly JwtSettings _jwtSettings;
        public JwtGenerator(IConfiguration configuration, RickAndMortyDbContext dbcontext, IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _configuration = configuration;
            _dbcontext = dbcontext;
        }

        public async Task<bool> ValidateToken(string token)
        {
            if (token == null)
                return false;

            //var dbToken = await _userManager.GetAuthenticationTokenAsync(await _userManager.FindByIdAsync(GetClaim(token,"id")), "MyApp", "AccessToken");
            var userId = GetClaim(token, "UserId");
            if (string.IsNullOrEmpty(userId))
                return false;

            var user = await _dbcontext.Users.Where(u => u.Id.ToString() == userId).FirstOrDefaultAsync();

            if (user == null)
                return false;

            //token = token.Split(' ')[1];
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }
        }


        public async Task<string> GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Name",user.Name)
                };

            var tokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Ttl"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: tokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
                return string.Empty;

            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}