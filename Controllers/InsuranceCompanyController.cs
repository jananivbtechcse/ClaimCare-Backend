using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.Models;
using AutoMapper;
using ClaimCare.DTOs.ClaimDTO;
using ClaimCare.Services.Interfaces;
using ClaimCare.DTOs.InsurancePlanDTO;
using System.Security.Claims;
using ClaimCare.DTOs.InsuranceCompanyDTO;
using ClaimCare.DTOs.PaginationDTO;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "InsuranceCompany")]
    public class InsuranceCompanyController : ControllerBase
    {
        private readonly IInsuranceCompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IClaimRepository _claimRepository;

        public InsuranceCompanyController(
            IInsuranceCompanyRepository repository,
            IMapper mapper,
            IClaimRepository claimRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _claimRepository = claimRepository;
        }

        
        [HttpPut("complete-profile")]
        public async Task<IActionResult> CompleteProfile(UpdateInsuranceCompanyDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var company = await _repository.GetCompanyByUserId(userId);

            if (company == null)
                return NotFound("Company not found");

            if (!string.IsNullOrEmpty(dto.CompanyName))
                company.CompanyName = dto.CompanyName;

            if (!string.IsNullOrEmpty(dto.Address))
                company.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.RegistrationNumber))
                company.RegistrationNumber = dto.RegistrationNumber;

            if (dto.IsActive.HasValue)
                company.IsActive = dto.IsActive.Value;

            await _repository.SaveAsync();

            return Ok("Insurance company profile updated successfully");
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateInsuranceCompanyDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var company = await _repository.GetCompanyByUserId(userId);

            if (company == null)
                return NotFound("Company not found");

            _mapper.Map(dto, company);

            await _repository.SaveAsync();

            return Ok("Insurance company profile updated successfully");
        }

        
        [HttpGet("claims")]
        public async Task<IActionResult> GetClaims()
        {
            var claims = await _claimRepository.GetAllClaimsAsync();

            var response = _mapper.Map<IEnumerable<ClaimResponseDTO>>(claims);

            return Ok(response);
        }
        [HttpGet("profile")]
public async Task<IActionResult> GetProfile()
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    var company = await _repository.GetCompanyByUserId(userId);

    if (company == null)
        return NotFound("Company not found");

    return Ok(new
    {
        companyName = company.CompanyName,
        email = User.FindFirst(ClaimTypes.Email)?.Value  // ✅ FIX HERE
    });
}
        
        [HttpPut("approve-claim/{id}")]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var claim = await _repository.GetClaimById(id);

            if (claim == null)
                return NotFound();

            claim.Status = "Approved";
            claim.ApprovedDate = DateTime.UtcNow;   // FIXED

            await _repository.SaveAsync();

            return Ok("Claim approved");
        }

        
        [HttpPut("reject-claim/{id}")]
        public async Task<IActionResult> RejectClaim(int id)
        {
            var claim = await _repository.GetClaimById(id);

            if (claim == null)
                return NotFound();

            claim.Status = "Rejected";
            claim.RejectionReason = "Rejected by Insurance Company"; // FIXED

            await _repository.SaveAsync();

            return Ok("Claim rejected");
        }

        
        [HttpGet("insurance-plans")]
        public async Task<IActionResult> GetPlans([FromQuery] PaginationParams pagination)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var company = await _repository.GetCompanyByUserId(userId);
            if (company == null)
                return NotFound("Company not found");

            var plans = await _repository.GetPlansByCompanyId(company.InsuranceCompanyId);

            var result = _mapper.Map<List<InsurancePlanResponseDTO>>(plans);

            return Ok(result);
        }

      
        [HttpPut("update-claim/{id}")]
        public async Task<IActionResult> UpdateClaim(int id, UpdateClaimStatusDTO dto)
        {
            var claim = await _repository.GetClaimById(id);   // FIXED (_context removed)

            if (claim == null)
                return NotFound();

            _mapper.Map(dto, claim);

            if (dto.Status == "Approved")
                claim.ApprovedDate = DateTime.UtcNow;

            await _repository.SaveAsync();

            var response = _mapper.Map<ClaimStatusResponseDTO>(claim);

            return Ok(response);
        }
    }
}