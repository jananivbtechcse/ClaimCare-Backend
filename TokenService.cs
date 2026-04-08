using ClaimCare.Models;
using ClaimCare.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly ClaimCareDbContext _context;

        public TokenService(IConfiguration config, ClaimCareDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateAccessToken(User user)
        {
           var claims = new[]
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(ClaimTypes.Role, user.Role.RoleName)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Temporary in-memory storage for refresh tokens
        private static Dictionary<string, string> _refreshTokens = new();

        public async Task SaveRefreshToken(string email, string refreshToken)
        {
            _refreshTokens[email] = refreshToken;
        }

        public async Task<string?> GetEmailFromRefreshToken(string refreshToken)
        {
            return _refreshTokens
                .FirstOrDefault(x => x.Value == refreshToken)
                .Key;
        }
    }
}