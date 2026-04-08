using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ClaimCare.Models;
using ClaimCare.DTOs.PaginationDTO;
using ClaimCare.DTOs.InsurancePlanDTO;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePlanController : ControllerBase
    {
        private readonly IInsurancePlanRepository _repository;
        private readonly IMapper _mapper;

        public InsurancePlanController(IInsurancePlanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

       
        [HttpGet]
        [Authorize(Roles = "Patient,InsuranceCompany")]
        public async Task<IActionResult> GetAllPlans([FromQuery] PaginationParams pagination)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            IEnumerable<InsurancePlan> plans;

            if (role == "InsuranceCompany")
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

                var company = await _repository.GetCompanyByUserId(userId);

                if (company == null)
                    return Unauthorized();

                plans = await _repository.GetPlansByCompany(company.InsuranceCompanyId);
            }
            else
            {
                plans = await _repository.GetAllActivePlans();
            }

            var pagedPlans = plans
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var result = _mapper.Map<List<InsurancePlanResponseDTO>>(pagedPlans);

            return Ok(result);
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,InsuranceCompany")]
        public async Task<IActionResult> GetPlan(int id)
        {
            var plan = await _repository.GetPlanById(id);

            if (plan == null)
                return NotFound();

            var result = _mapper.Map<InsurancePlanResponseDTO>(plan);

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(Roles = "InsuranceCompany")]
        public async Task<IActionResult> CreatePlan(CreateInsurancePlanDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var company = await _repository.GetCompanyByUserId(userId);

            if (company == null)
                return Unauthorized();

            var plan = _mapper.Map<InsurancePlan>(dto);

            plan.InsuranceCompanyId = company.InsuranceCompanyId;

            await _repository.AddPlan(plan);

            await _repository.SaveAsync();

            return Ok("Insurance plan created successfully");
        }

        
        [HttpPut("{id}")]
[Authorize(Roles = "InsuranceCompany")]
public async Task<IActionResult> UpdatePlan(int id, UpdateInsurancePlanDto dto)
{
    if (dto == null)
        return BadRequest("dto is required");

    
    var plan = await _repository.GetPlanById(id);
    if (plan == null)
        return NotFound();

   
    if (!string.IsNullOrEmpty(dto.PlanName))
        plan.PlanName = dto.PlanName;

    if (dto.CoverageAmount.HasValue)
        plan.CoverageAmount = dto.CoverageAmount.Value;

    if (dto.PremiumAmount.HasValue)
        plan.PremiumAmount = dto.PremiumAmount.Value;

    if (!string.IsNullOrEmpty(dto.PlanDescription))
        plan.PlanDescription = dto.PlanDescription;

    if (dto.IsActive.HasValue)
        plan.IsActive = dto.IsActive.Value;

    
    await _repository.SaveAsync();

    return Ok("Plan updated successfully");
}
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "InsuranceCompany")]
        public async Task<IActionResult> DeactivatePlan(int id)
        {
            var plan = await _repository.GetPlanById(id);

            if (plan == null)
                return NotFound();

            plan.IsActive = false;

            await _repository.UpdatePlan(plan);

            await _repository.SaveAsync();

            return Ok("Insurance plan deactivated");
        }
    }
}