using AuthenticationCL.Domain;
using AuthenticationCL.IServices;
using System.Security.Claims;
using System.Text;

namespace AuthenticationCL.Services
{
        public class JwtService : IJwtService
        {
            private readonly IConfiguration _config;

            public JwtService(IConfiguration config)
            {
                _config = config;
            }

            public string GenerateToken(User user)
            {
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(3),
                    Audience = _config["Jwt:Audience"],
                    Issuer = _config["Jwt:Issuer"],
                    SigningCredentials = credentials
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
}