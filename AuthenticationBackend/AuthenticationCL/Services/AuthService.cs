using AuthenticationCL.Domain.Data;
using AuthenticationCL.Domain.Entities;
using AuthenticationCL.DTOs;
using AuthenticationCL.IServices;
using AuthenticationCL.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationCL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        //private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration/*, IJwtService jwtService*/)
        {
            _context = context;
            //_jwtService = jwtService;
            _configuration = configuration;
        }

        // JWT GENERATOR
        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // REFRESH TOKEN GENERATOR
        public Task<string> GenerateRefreshTokenAsync(string email)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return Task.FromResult(token);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequestDTO request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Username = user.Username,
                Role = user.Role,
                AccessToken = GenerateJwt(user),
                RefreshToken = await GenerateRefreshTokenAsync(user.Email)
            };
        }

        public async Task<AuthResponse> LoginAsync(LogInRequestDTO request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !PasswordHelper.VerifyPassword(user.PasswordHash, request.Password))
                throw new Exception("Invalid credentials.");

            return new AuthResponse
            {
                Username = user.Username,
                Role = user.Role,
                AccessToken = GenerateJwt(user),
                RefreshToken = await GenerateRefreshTokenAsync(user.Email)
            };
        }
       
    }

}
