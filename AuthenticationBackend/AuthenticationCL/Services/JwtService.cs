using AuthenticationCL.Domain;
using AuthenticationCL.Domain.Entities;
using AuthenticationCL.IServices;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationCL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly IDistributedCache _cache;

        public JwtService(IConfiguration config, IDistributedCache cache)
        {
            _config = config;
            _cache = cache;
        }

        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            string token = Guid.NewGuid().ToString("N");

            await _cache.SetStringAsync(
                $"refresh:{userId}",
                token,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(
                        double.Parse(_config["Jwt:RefreshTokenExpiresDays"])
                    )
                });

            return token;
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string providedToken)
        {
            var stored = await _cache.GetStringAsync($"refresh:{userId}");
            return stored == providedToken;
        }

        public async Task RevokeRefreshTokenAsync(Guid userId)
        {
            await _cache.RemoveAsync($"refresh:{userId}");
        }
    }
}