// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using ClaimCare.Services.Interfaces;
// using ClaimCare.DTOs.PaginationDTO;
// using ClaimCare.DTOs.UserDTO;
// using AutoMapper;

// namespace ClaimCare.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize(Roles = "Admin")]
//     public class UserController : ControllerBase
//     {
//         private readonly IUserRepository _userRepository;
//         private readonly IMapper _mapper;

//         public UserController(IUserRepository userRepository, IMapper mapper)
//         {
//             _userRepository = userRepository;
//             _mapper = mapper;
//         }

//         [HttpGet("users")]
//         [Authorize(Roles = "Admin")]
//         public async Task<IActionResult> GetUsers([FromQuery] PaginationParams pagination)
//         {
//             var users = await _userRepository.GetAllUsers(pagination);

//             var result = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

//             return Ok(result);
//         }

        
//         [HttpGet("patients")]
//         [Authorize(Roles = "Admin,HealthcareProvider")]
//         public async Task<IActionResult> GetPatients([FromQuery] PaginationParams pagination)
//         {
//             var patients = await _userRepository.GetAllPatients(pagination);

//             var result = _mapper.Map<IEnumerable<UserResponseDTO>>(patients);

//             return Ok(result);
//         }

    
//         [HttpGet("providers")]
//         public async Task<IActionResult> GetProviders([FromQuery] PaginationParams pagination)
//         {
//             var providers = await _userRepository.GetAllProviders(pagination);

//             var result = _mapper.Map<IEnumerable<UserResponseDTO>>(providers);

//             return Ok(result);
//         }

       
//         [HttpGet("insurance-companies")]
//         public async Task<IActionResult> GetInsuranceCompanies([FromQuery] PaginationParams pagination)
//         {
//             var companies = await _userRepository.GetAllInsuranceCompanies(pagination);

//             var result = _mapper.Map<IEnumerable<UserResponseDTO>>(companies);

//             return Ok(result);
//         }

     
//         [HttpDelete("delete-user/{id}")]
//         public async Task<IActionResult> DeleteUser(int id)
//         {
//             var user = await _userRepository.GetUserById(id);

//             if (user == null)
//                 return NotFound("User not found");

//             await _userRepository.DeleteUser(id);

//             return Ok("User deleted successfully");
//         }
//     }
// }


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using ClaimCare.DTOs.PaginationDTO;
using ClaimCare.DTOs.UserDTO;
using ClaimCare.DTOs.AuthDTO;
using ClaimCare.Services;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ClaimCareDbContext _context;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, ClaimCareDbContext context, IMapper mapper)
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        // ─────────────────────────────────────────────
        // GET ENDPOINTS
        // ─────────────────────────────────────────────

        // [HttpGet("users")]
        // public async Task<IActionResult> GetUsers([FromQuery] PaginationParams pagination)
        // {
        //     var users = await _userRepository.GetAllUsers(pagination);
        //     var result = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        //     return Ok(result);
        // }

[HttpGet("users")]
public async Task<IActionResult> GetUsers([FromQuery] PaginationParams pagination)
{
    // Get total count BEFORE pagination
    var totalCount = await _context.Users.CountAsync();
    
    var users = await _userRepository.GetAllUsers(pagination);
    var result = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    
    return Ok(new {
        items      = result,
        totalCount = totalCount,
        pageNumber = pagination.PageNumber,
        pageSize   = pagination.PageSize
    });
}
        [HttpGet("patients")]
        [Authorize(Roles = "Admin,HealthcareProvider")]
        public async Task<IActionResult> GetPatients([FromQuery] PaginationParams pagination)
        {
            var patients = await _userRepository.GetAllPatients(pagination);
            var result = _mapper.Map<IEnumerable<UserResponseDTO>>(patients);
            return Ok(result);
        }

        [HttpGet("providers")]
        [Authorize(Roles="Admin,Patient")]
        public async Task<IActionResult> GetProviders([FromQuery] PaginationParams pagination)
        {
            var providers = await _userRepository.GetAllProviders(pagination);
            var result = _mapper.Map<IEnumerable<UserResponseDTO>>(providers);
            return Ok(result);
        }

        [HttpGet("insurance-companies")]
        public async Task<IActionResult> GetInsuranceCompanies([FromQuery] PaginationParams pagination)
        {
            var companies = await _userRepository.GetAllInsuranceCompanies(pagination);
            var result = _mapper.Map<IEnumerable<UserResponseDTO>>(companies);
            return Ok(result);
        }

        // ─────────────────────────────────────────────
        // POST: Admin creates Provider / Insurance / Admin accounts
        // ─────────────────────────────────────────────

        [HttpPost("create-user")]
public async Task<IActionResult> CreateUser([FromBody] ClaimCare.DTOs.AuthDTO.RegisterUserDTO request)
        {
            // 🔒 Admin cannot create Patient accounts via this endpoint
            // Patients self-register via /api/Auth/register
            if (request.RoleName == "Patient")
                return BadRequest("Patients must self-register via /api/Auth/register");

            if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                return BadRequest("Email already exists");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == request.RoleName);

            if (role == null)
                return BadRequest($"Invalid role: {request.RoleName}. Valid roles: Provider, Insurance, Admin");

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

            // Auto-create role-specific profile
            if (role.RoleName == "Provider")
            {
                _context.HealthcareProviders.Add(new HealthcareProvider
                {
                    UserId = user.UserId,
                    HospitalName = request.FullName,
                    LicenseNumber = "TEMP-" + Guid.NewGuid().ToString()[..8],
                    Address = "Not Provided"
                });
            }
            else if (role.RoleName == "Insurance")
            {
                _context.InsuranceCompanies.Add(new InsuranceCompany
                {
                    UserId = user.UserId,
                    CompanyName = request.FullName,
                    RegistrationNumber = "TEMP-" + Guid.NewGuid().ToString()[..8],
                    Address = "Not Provided"
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{role.RoleName} account created successfully", userId = user.UserId });
        }

        // ─────────────────────────────────────────────
        // PUT: Admin updates any user's basic info
        // ─────────────────────────────────────────────

        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found");

            // Update only the fields provided
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Password))
                user.PasswordHash = HashPassword(request.Password);

            // Toggle active/inactive
            if (request.IsActive.HasValue)
                user.IsActive = request.IsActive.Value;

            await _context.SaveChangesAsync();

            return Ok("User updated successfully");
        }

        // ─────────────────────────────────────────────
        // DELETE: Admin removes any user
        // ─────────────────────────────────────────────

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
                return NotFound("User not found");

            await _userRepository.DeleteUser(id);

            return Ok("User deleted successfully");
        }

        // ─────────────────────────────────────────────
        // HELPER
        // ─────────────────────────────────────────────

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}