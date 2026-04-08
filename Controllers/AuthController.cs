
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClaimCare.Data;
using ClaimCare.Models;

using ClaimCare.DTOs.AuthDTO;
using ClaimCare.Services;
using System.Security.Cryptography;
using System.Text;

namespace ClaimCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ClaimCareDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(ClaimCareDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // ✅ PUBLIC: Only Patient can self-register
        // [HttpPost("register")]
        // [Authorize(Roles="Admin,Patient")]
        // public async Task<IActionResult> Register(RegisterUserDTO request)
        // {
        //     if (await _context.Users.AnyAsync(x => x.Email == request.Email))
        //         return BadRequest("Email already exists");

        //     // 🔒 Force role to "Patient" — ignore whatever the frontend sends
        //     var role = await _context.Roles
        //         .FirstOrDefaultAsync(r => r.RoleName == "Patient");

        //     if (role == null)
        //         return BadRequest("Patient role not found. Please contact admin.");

        //     var passwordHash = HashPassword(request.Password);

        //     var user = new User
        //     {
        //         FullName = request.FullName,
        //         Email = request.Email,
        //         PasswordHash = passwordHash,
        //         PhoneNumber = request.PhoneNumber,
        //         RoleId = role.RoleId,
        //         CreatedAt = DateTime.UtcNow,
        //         IsActive = true
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();

        //     // Auto-create Patient profile
        //     var patient = new Patient
        //     {
        //         UserId = user.UserId,
        //         DateOfBirth = DateTime.UtcNow,
        //         Gender = "Unknown",
        //         Address = "Not Provided",
        //         Symptoms = "Not Provided"
        //     };

        //     _context.Patients.Add(patient);
        //     await _context.SaveChangesAsync();

        //     return Ok("Patient account created successfully");
        // }


            [HttpPost("register")]
[AllowAnonymous]   // ✅ FIXED
public async Task<IActionResult> Register(RegisterUserDTO request)
{
    if (await _context.Users.AnyAsync(x => x.Email == request.Email))
        return BadRequest("Email already exists");

    // 🔒 Force role to "Patient"
    var role = await _context.Roles
        .FirstOrDefaultAsync(r => r.RoleName == "Patient");

    if (role == null)
        return BadRequest("Patient role not found. Please contact admin.");

    var passwordHash = HashPassword(request.Password);

    var user = new User
    {
        FullName = request.FullName,
        Email = request.Email,
        PasswordHash = passwordHash,
        PhoneNumber = request.PhoneNumber,
        RoleId = role.RoleId,
        CreatedAt = DateTime.UtcNow,
        IsActive = true
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Auto-create Patient profile
    var patient = new Patient
    {
        UserId = user.UserId,
        DateOfBirth = DateTime.UtcNow,
        Gender = "Unknown",
        Address = "Not Provided",
        Symptoms = "Not Provided"
    };

    _context.Patients.Add(patient);
    await _context.SaveChangesAsync();

    return Ok("Patient account created successfully");
}
        // ✅ PUBLIC: Login for all roles
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                return Unauthorized("Invalid email");

            if (!VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid password");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await _tokenService.SaveRefreshToken(user.Email, refreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = user.Role.RoleName
            });
        }

        // ✅ PUBLIC: Refresh token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var email = await _tokenService.GetEmailFromRefreshToken(request.RefreshToken);

            if (email == null)
                return Unauthorized();

            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return Unauthorized();

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await _tokenService.SaveRefreshToken(email, newRefreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash) == storedHash;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}