using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apposite.Application.Settings;
using Apposite.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Apposite.Domain.Models;
using Microsoft.Extensions.Options;

namespace Apposite.Application.Services
{
    public class JwtGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<JwtGenerator> _logger;

        public JwtGenerator(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager, ILogger<JwtGenerator> logger)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }
        public async Task<ValidateTokenResult> ValidateToken(string token, TokenType tokenType = TokenType.AccessToken)
        {
            if (token == null)
                return new ValidateTokenResult(false, "Please provide valid token!");

            //var dbToken = await _userManager.GetAuthenticationTokenAsync(await _userManager.FindByIdAsync(GetClaim(token,"id")), "MyApp", "AccessToken");
            var userId = GetClaim(token, "id");
            if (string.IsNullOrEmpty(userId))
                return new ValidateTokenResult(false, "Token not valid! Please login to get new token!");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ValidateTokenResult(false, "Token not found!");
            }
            var result = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", tokenType.ToString());
            if (!token.Equals(result))
            {
                return new ValidateTokenResult(false, "Token not valid! Please login to get new token!");
            }

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

                return new ValidateTokenResult(true, string.Empty, GetClaim(token, "id"));
            }
            catch (SecurityTokenExpiredException)
            {
                return new ValidateTokenResult(false, "Token has expired! Please login to get new token!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(JwtGenerator)} throw an exception. Exception: {e.Message}", e);
                return new ValidateTokenResult(false, e.Message);
            }
        }

        public async Task<string> GenerateJwt(User user)
        {
            string token = "";

            try
            {
                var listOfRoles = await _userManager.GetRolesAsync(user);
                string rolesTxt = "Public";
                if (listOfRoles.Count > 0)
                    rolesTxt = listOfRoles.Aggregate((a, b) => a + "," + b);
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("id", user.Id.ToString()),
            new Claim("role", rolesTxt)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var rawToken = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddSeconds(_jwtSettings.Ttl),
                    signingCredentials: signIn);

                token = new JwtSecurityTokenHandler().WriteToken(rawToken);
                await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "AccessToken", token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GenerateJwt)} throw an exception. Exception: {ex.Message}", ex);
            }

            //var email = await _userManager.GenerateUserTokenAsync(user, "Email", "MyApp");
            //var sms = await _userManager.GenerateUserTokenAsync(user, "SMS", "MyApp");

            return token;
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            string token = "";
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var rawToken = new JwtSecurityToken(
                    null,
                    null,
                    expires: DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshTtl),
                    signingCredentials: signIn,
                    claims: new Claim[] { new Claim("id", user.Id.ToString()) });
                token = new JwtSecurityTokenHandler().WriteToken(rawToken);
                await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GenerateRefreshToken)} throw an exception. Exception: {ex.Message}", ex);
            }

            return token;

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

        public async Task Logout(User user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "AccessToken");
            await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
        }

        public enum TokenType
        {
            RefreshToken, AccessToken
        };

    }
}