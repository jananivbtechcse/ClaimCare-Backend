using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.Data;
using ClaimCare.DTOs.InsurancePlanDTO;
using ClaimCare.DTOs.InvoiceDTO;
using ClaimCare.DTOs.PatientDTO;
using ClaimCare.DTOs.ClaimDTO;
using AutoMapper;
using System.Security.Claims;
using ClaimCare.Models;
using Microsoft.EntityFrameworkCore;
using ClaimCare.DTOs;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly ClaimCareDbContext _context;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
         private readonly EmailService _emailService;

        public PatientController(
            ClaimCareDbContext context,
            IPatientRepository patientRepository,
            IMapper mapper,
            EmailService emailService)
        {
            _context = context;
            _patientRepository = patientRepository;
            _mapper = mapper;
             _emailService = emailService;
        }

        [HttpPut("complete-profile")]
    public async Task<IActionResult> CompleteProfile(UpdatePatientDTO dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var patient = await _patientRepository.GetPatientByUserIdAsync(userId);

    if (patient == null)
        return NotFound("Patient not found");

   
    _mapper.Map(dto, patient);

   
    if (dto.InsurancePlanId.HasValue && dto.InsurancePlanId != 0)
    {
        patient.InsurancePlanId = dto.InsurancePlanId;
    }
    else
    {
        patient.InsurancePlanId = null;
    }

    await _patientRepository.UpdatePatientAsync(patient);

    return Ok("Patient profile completed successfully" );
}

       
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var patient = await _patientRepository.GetPatientWithUserAsync(userId);

            if (patient == null)
                return NotFound("Patient not found");

            var response = _mapper.Map<PatientDetailDto>(patient);

            return Ok(response);
        }

       
       [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdatePatientDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var patient = await _patientRepository.GetPatientByUserIdAsync(userId);

            if (patient == null)
                return NotFound("Patient not found");

            _mapper.Map(dto, patient); 
            await _patientRepository.UpdatePatientAsync(patient);

            return Ok( "Patient profile updated successfully");
        }
        
        [HttpGet("insurance-plans")]
        [Authorize(Roles = "Patients")]
        public async Task<IActionResult> GetInsurancePlans()
        {
            var plans = await _context.InsurancePlans
                .Include(p => p.InsuranceCompany)
                .ToListAsync();

            var result = _mapper.Map<List<InsurancePlanResponseDTO>>(plans);

            return Ok(result);
        }
        // In your PatientController or a new PublicController
// [HttpGet("providers/list")]
// [Authorize(Roles = "Patient")]
// public async Task<IActionResult> GetProviderList()
// {
//     var providers = await _context.HealthcareProviders
//         .Include(p => p.User)
//         .Select(p => new {
//             providerId   = p.ProviderId,   // ← the PK request-invoice needs
//             providerName = p.User.FullName ?? p.HospitalName
//         })
//         .ToListAsync();

//     return Ok(providers);
// }

[HttpGet("providers/list")]
[Authorize(Roles = "Patient")]
public async Task<IActionResult> GetProviderList()
{
    var providers = await _context.Users
        .Include(u => u.Role)
        .Where(u => u.Role.RoleName == "HealthcareProvider" && u.IsActive)
        .Select(u => new {
            providerId   = u.UserId,    // UserId — used by RequestInvoice to match
            providerName = u.FullName
        })
        .ToListAsync();

    return Ok(providers);
}

       
        [HttpPost("select-plan/{planId}")]
        public async Task<IActionResult> SelectInsurancePlan(int planId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var patient = await _patientRepository.GetPatientByUserIdAsync(userId);

            if (patient == null)
                return NotFound("Patient not found");

            var plan = await _context.InsurancePlans.FindAsync(planId);

            if (plan == null)
                return NotFound("Insurance plan not found");

            patient.InsurancePlanId = planId;

            await _patientRepository.UpdatePatientAsync(patient);

            return Ok( "Insurance plan selected successfully" );
        }

   
        [HttpGet("invoices")]
        public async Task<IActionResult> GetInvoices()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var invoices = await _context.Invoices
                .Include(i => i.Patient)
                    .ThenInclude(p => p.User)
                .Include(i => i.Provider)
                .Where(i => i.Patient.UserId == userId)
                .ToListAsync();

            var result = _mapper.Map<List<InvoiceResponseDTO>>(invoices);

            return Ok(result);
        }

     
        // [HttpPost("submit-claim")]
        // [Authorize(Roles = "Patient")]
        // public async Task<IActionResult> SubmitClaim(CreateClaimDTO dto)
        // {
        //     var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        //     var patient = await _patientRepository.GetPatientWithUserAsync(userId);

        //     if (patient == null)
        //         return NotFound("Patient not found");

        //     var claim = _mapper.Map<ClaimCare.Models.Claim>(dto);
        //     claim.PatientId = patient.PatientId;
        //     claim.ClaimNumber = $"CLM-{DateTime.Now:yyyyMMddHHmmss}";
        //     claim.Status = "Pending";
        //     claim.SubmissionDate = DateTime.UtcNow;

        //     _context.Claims.Add(claim);
        //     await _context.SaveChangesAsync();

         
        //     var insuranceUsers = await _context.Users
        //         .Include(u => u.Role)
        //         .Where(u => u.Role.RoleName == "InsuranceCompany")
        //         .ToListAsync();

        //     foreach (var user in insuranceUsers)
        //     {
        //         _context.Notifications.Add(new Notification
        //         {
        //             UserId = user.UserId,
        //             Message = $"New claim {claim.ClaimNumber} submitted by patient.",
        //             CreatedAt = DateTime.UtcNow,
        //             IsEmailSent = false
        //         });

              
        //         await _emailService.SendEmailAsync(
        //             user.Email,
        //             "New Claim Submitted",
        //             $"A new claim {claim.ClaimNumber} has been submitted by patient {patient.User.FullName}."
        //         );
        //     }

        //     _context.Notifications.Add(new Notification
        //     {
        //         UserId = patient.UserId,
        //         Message = $"Your claim {claim.ClaimNumber} has been submitted successfully.",
        //         CreatedAt = DateTime.UtcNow,
        //         IsEmailSent = true
        //     });

            
        //     await _emailService.SendEmailAsync(
        //         patient.User.Email,
        //         "Claim Submitted",
        //         $"Your claim {claim.ClaimNumber} has been submitted successfully and is under review."
        //     );

        //     await _context.SaveChangesAsync();

        //     return Ok("Claim submitted successfully");
        // }


        [HttpPost("submit-claim")]
[Authorize(Roles = "Patient")]
public async Task<IActionResult> SubmitClaim(CreateClaimDTO dto)
{
    var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    var patient = await _patientRepository.GetPatientWithUserAsync(userId);

    if (patient == null)
        return NotFound("Patient not found");

    var claim = _mapper.Map<ClaimCare.Models.Claim>(dto);
    claim.PatientId = patient.PatientId;
    claim.ClaimNumber = $"CLM-{DateTime.Now:yyyyMMddHHmmss}";
    claim.Status = "Pending";
    claim.SubmissionDate = DateTime.UtcNow;

    _context.Claims.Add(claim);

    // ✅ Fetch insurance users
    var insuranceUsers = await _context.Users
        .Include(u => u.Role)
        .Where(u => u.Role.RoleName == "InsuranceCompany")
        .ToListAsync();

    // ✅ Add ALL notifications in one batch (no emails yet)
    foreach (var user in insuranceUsers)
    {
        _context.Notifications.Add(new Notification
        {
            UserId = user.UserId,
            Message = $"New claim {claim.ClaimNumber} submitted by patient.",
            CreatedAt = DateTime.UtcNow,
            IsEmailSent = false
        });
    }

    _context.Notifications.Add(new Notification
    {
        UserId = patient.UserId,
        Message = $"Your claim {claim.ClaimNumber} has been submitted successfully.",
        CreatedAt = DateTime.UtcNow,
        IsEmailSent = true
    });


    await _context.SaveChangesAsync();

    
    _ = Task.Run(async () =>
    {
        var emailTasks = insuranceUsers.Select(user =>
            _emailService.SendEmailAsync(
                user.Email,
                "New Claim Submitted",
                $"A new claim {claim.ClaimNumber} has been submitted by patient {patient.User.FullName}."
            )
        ).ToList();

        emailTasks.Add(_emailService.SendEmailAsync(
            patient.User.Email,
            "Claim Submitted",
            $"Your claim {claim.ClaimNumber} has been submitted successfully and is under review."
        ));

        await Task.WhenAll(emailTasks);
    });

    return Ok("Claim submitted successfully");
}



[HttpPost("request-invoice")]
[Authorize(Roles = "Patient")]
public async Task<IActionResult> RequestInvoice([FromBody] CreateInvoiceRequestDTO dto)
{
    if (dto == null) return BadRequest("Invalid request");

    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    if (userIdClaim == null) return Unauthorized("Invalid user");

    var userId = int.Parse(userIdClaim.Value);

    var patient = await _patientRepository.GetPatientByUserIdAsync(userId);
    if (patient == null) return NotFound("Patient not found");

    // Look up provider by UserId (what the dropdown sends)
    var provider = await _context.HealthcareProviders
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.UserId == dto.ProviderId);

    // If no HealthcareProviders row yet, fall back to just the User record
    if (provider == null)
    {
        var providerUser = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == dto.ProviderId
                                   && u.Role.RoleName == "HealthcareProvider");

        if (providerUser == null)
            return NotFound("Provider not found");

       
provider = new HealthcareProvider
{
    UserId = providerUser.UserId,
    HospitalName = providerUser.FullName,
    Address = "N/A",
    LicenseNumber = "TEMP-" + Guid.NewGuid().ToString().Substring(0, 8)
};
        _context.HealthcareProviders.Add(provider);
        await _context.SaveChangesAsync();
    }

    var request = new InvoiceRequest
    {
        PatientId  = patient.PatientId,
        ProviderId = provider.ProviderId,
        VisitDate  = dto.VisitDate,
        Status     = "Pending",
        CreatedAt  = DateTime.UtcNow
    };

    _context.InvoiceRequests.Add(request);
    await _context.SaveChangesAsync();

    var patientName = patient.User?.FullName ?? "Patient";
    _context.Notifications.Add(new Notification
    {
        UserId    = provider.UserId,
        Message   = $"Patient {patientName} has requested an invoice.",
        CreatedAt = DateTime.UtcNow
    });

    await _context.SaveChangesAsync();
    return Ok("Invoice request submitted successfully");
}
[HttpGet("my-invoice-requests")]
[Authorize(Roles = "Patient")]
public async Task<IActionResult> GetMyInvoiceRequests()
{
    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Unauthorized("Invalid user");

    var userId = int.Parse(userIdClaim.Value);

    var patient = await _patientRepository.GetPatientByUserIdAsync(userId);
    if (patient == null)
        return NotFound("Patient not found");

    var requests = await _context.InvoiceRequests
    .Include(r => r.Patient)                 // ✅ ADD THIS
        .ThenInclude(p => p.User)           // ✅ ADD THIS
    .Include(r => r.Provider)
        .ThenInclude(p => p.User)
    .Where(r => r.PatientId == patient.PatientId)
    .OrderByDescending(r => r.CreatedAt)
    .ToListAsync();

    var result = _mapper.Map<List<InvoiceRequestResponseDTO>>(requests);

    return Ok(result);
}
    }
}