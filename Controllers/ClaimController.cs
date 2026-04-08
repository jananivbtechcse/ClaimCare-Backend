using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.DTOs.ClaimDTO;
using ClaimCare.Data;
using System.Security.Claims;
using ClaimCare.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ClaimCare.DTOs.PaginationDTO;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimRepository _claimRepository;
        private readonly ClaimCareDbContext _context;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;

        public ClaimController(
            IClaimRepository claimRepository,
            ClaimCareDbContext context,
            IMapper mapper,
            EmailService emailService)
        {
            _claimRepository = claimRepository;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        // PATIENT → VIEW MY CLAIMS
        [HttpGet("my-claims")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyClaims()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
                return NotFound("Patient profile not found");

            var claims = await _claimRepository.GetClaimsByPatientAsync(patient.PatientId);

            var claimDtos = _mapper.Map<List<ClaimResponseDTO>>(claims);

            return Ok(claimDtos);
        }

        // INSURANCE COMPANY → VIEW ALL CLAIMS
        // [HttpGet]
        // [Authorize(Roles = "InsuranceCompany,Admin")]
        // public async Task<IActionResult> GetAllClaims([FromQuery] PaginationParams pagination)
        // {
        //     var claims = await _claimRepository.GetAllClaimsAsync();

        //     var claimDtos = _mapper.Map<List<ClaimResponseDTO>>(claims);

        //     return Ok(claimDtos);
        // }

// [HttpGet]
// [Authorize(Roles = "InsuranceCompany,Admin")]
// public async Task<IActionResult> GetAllClaims([FromQuery] PaginationParams pagination)
// {
//     // ✅ Load all into memory first, then paginate — fixes old SQL Server OFFSET error
//     var allClaims = await _context.Claims
//         .OrderByDescending(c => c.SubmissionDate)
//         .ToListAsync();

//     var totalCount = allClaims.Count;

//     var paged = allClaims
//         .Skip((pagination.PageNumber - 1) * pagination.PageSize)
//         .Take(pagination.PageSize)
//         .ToList();

//     var claimDtos = _mapper.Map<List<ClaimResponseDTO>>(paged);

//     return Ok(new {
//         items      = claimDtos,
//         totalCount = totalCount,
//         pageNumber = pagination.PageNumber,
//         pageSize   = pagination.PageSize
//     });
// }


[HttpGet]
[Authorize(Roles = "InsuranceCompany,Admin")]
public async Task<IActionResult> GetAllClaims([FromQuery] PaginationParams pagination)
{
    var allClaims = await _context.Claims
        .Include(c => c.Invoice)      // ← ADD THIS
        .Include(c => c.Patient)      // ← ADD THIS (for safety)
            .ThenInclude(p => p.User)
        .OrderByDescending(c => c.SubmissionDate)
        .ToListAsync();

    var totalCount = allClaims.Count;

    var paged = allClaims
        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
        .Take(pagination.PageSize)
        .ToList();

    var claimDtos = _mapper.Map<List<ClaimResponseDTO>>(paged);

    return Ok(new {
        items      = claimDtos,
        totalCount = totalCount,
        pageNumber = pagination.PageNumber,
        pageSize   = pagination.PageSize
    });
}

        
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "InsuranceCompany")]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(id);
            if (claim == null) return NotFound("Claim not found");

            claim.Status = "Approved";
            claim.ApprovedDate = DateTime.UtcNow;

            await _claimRepository.UpdateClaimAsync(claim);

            var patientUser = claim.Patient.User;

            // Notification
            _context.Notifications.Add(new Notification
            {
                UserId = patientUser.UserId,
                Message = $"Your claim {claim.ClaimNumber} has been approved.",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Email
            await _emailService.SendEmailAsync(
                patientUser.Email,
                "Claim Approved",
                $"Your claim {claim.ClaimNumber} has been approved successfully."
            );

            return Ok("Claim approved successfully");
        }

        // -------------------------
        // INSURANCE COMPANY → REJECT CLAIM
        // -------------------------
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "InsuranceCompany")]
        public async Task<IActionResult> RejectClaim(int id, string reason)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Rejected";
            claim.RejectionReason = reason;

            await _claimRepository.UpdateClaimAsync(claim);

            var patientUser = claim.Patient.User;

            // Notification
            _context.Notifications.Add(new Notification
            {
                UserId = patientUser.UserId,
                Message = $"Your claim {claim.ClaimNumber} was rejected. Reason: {reason}",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Email
            await _emailService.SendEmailAsync(
                patientUser.Email,
                "Claim Rejected",
                $"Your claim {claim.ClaimNumber} was rejected. Reason: {reason}"
            );

            return Ok("Claim rejected");
        }
    }
}