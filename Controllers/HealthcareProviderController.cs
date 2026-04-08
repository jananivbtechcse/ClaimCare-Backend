using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.Data;
using ClaimCare.Services.Interfaces;
using ClaimCare.DTOs.InvoiceDTO;
using ClaimCare.DTOs.HealthcareProviderDTO;
using ClaimCare.DTOs.ClaimDTO;
using ClaimCare.Models;
using AutoMapper;
using ClaimCare.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ClaimCare.DTOs.PaginationDTO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

using System.Net;
using System.Net.Mail;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HealthcareProvider")]
    public class HealthcareProviderController : ControllerBase
    {
        private readonly ClaimCareDbContext _context;
        private readonly IHealthcareProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly IClaimRepository _claimRepository;

        private readonly EmailService _emailService;

public HealthcareProviderController(
    ClaimCareDbContext context,
    IHealthcareProviderRepository providerRepository,
    IClaimRepository claimRepository,
    IMapper mapper,
    EmailService emailService)  // Add this
{
    _context = context;
    _providerRepository = providerRepository;
    _claimRepository = claimRepository;
    _mapper = mapper;
    _emailService = emailService; // Assign
}

       
        [HttpPut("complete-profile")]
        public async Task<IActionResult> CompleteProfile(CreateHealthcareProviderDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var provider = await _context.HealthcareProviders
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (provider == null)
                return NotFound("Provider not found");

            provider.HospitalName = dto.HospitalName;
            provider.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok("Healthcare provider profile updated successfully");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var provider = await _context.HealthcareProviders
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.UserId == userId);

            if (provider == null)
                return NotFound();

            var response = _mapper.Map<HealthcareProviderResponseDTO>(provider);

            return Ok(response);
        }

        // UPDATE PROVIDER PROFILE
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateHealthcareProviderDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var provider = await _context.HealthcareProviders
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (provider == null)
                return NotFound("Provider not found");

            provider.HospitalName = dto.HospitalName;
            provider.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok("Healthcare provider profile updated successfully");
        }

        
        // [HttpPost("create-invoice")]
        // public async Task<IActionResult> CreateInvoice(CreateInvoiceDTO dto)
        // {
        //     var invoice = _mapper.Map<Invoice>(dto);

        //     invoice.InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}";
        //     invoice.CreatedAt = DateTime.UtcNow;
        //     invoice.InvoiceDate = DateTime.UtcNow;
        //     invoice.DueDate = DateTime.UtcNow.AddDays(7);

            
        //     invoice.TotalAmount = dto.ConsultationFee
        //                         + dto.DiagnosticTestFee
        //                         + dto.DiagnosticScanFee
        //                         + dto.MedicineFee;

        //     invoice.TaxAmount = (invoice.TotalAmount * dto.TaxPercentage) / 100;

        //     _context.Invoices.Add(invoice);

        //     await _context.SaveChangesAsync();

        //     return Ok("Invoice created successfully");
        // }

       [HttpPost("create-invoice")]
public async Task<IActionResult> CreateInvoice(CreateInvoiceDTO dto)
{
    var invoice = _mapper.Map<Invoice>(dto);

    invoice.InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}";
    invoice.CreatedAt = DateTime.UtcNow;
    invoice.InvoiceDate = DateTime.UtcNow;
    invoice.DueDate = DateTime.UtcNow.AddDays(7);

    invoice.TotalAmount = dto.ConsultationFee
                        + dto.DiagnosticTestFee
                        + dto.DiagnosticScanFee
                        + dto.MedicineFee;

    invoice.TaxAmount = (invoice.TotalAmount * dto.TaxPercentage) / 100;

    _context.Invoices.Add(invoice);
    await _context.SaveChangesAsync();

    var patient = await _context.Patients
        .Include(p => p.User)
        .FirstOrDefaultAsync(p => p.PatientId == dto.PatientId);

    if (patient != null)
    {
        var emailService = new EmailService();

        string body = $@"
<html>
<body>
<h2>Hello {patient.User.FullName},</h2>
<p>Your invoice has been generated by your Healthcare Provider:</p>

<table border='1' cellpadding='10' cellspacing='0' style='border-collapse: collapse; width: 50%;'>
<tr><th>Invoice Number</th><td>{invoice.InvoiceNumber}</td></tr>
<tr><th>Consultation Fee</th><td>{dto.ConsultationFee:C}</td></tr>
<tr><th>Diagnostic Test Fee</th><td>{dto.DiagnosticTestFee:C}</td></tr>
<tr><th>Diagnostic Scan Fee</th><td>{dto.DiagnosticScanFee:C}</td></tr>
<tr><th>Medicine Fee</th><td>{dto.MedicineFee:C}</td></tr>
<tr><th>Tax Percentage</th><td>{dto.TaxPercentage}%</td></tr>
<tr><th>Tax Amount</th><td>{invoice.TaxAmount:C}</td></tr>
<tr><th>Total Amount</th><td>{invoice.TotalAmount:C}</td></tr>
<tr><th>Invoice Date</th><td>{invoice.InvoiceDate:dd/MM/yyyy}</td></tr>
<tr><th>Due Date</th><td>{invoice.DueDate:dd/MM/yyyy}</td></tr>
</table>

<p>Thank you for choosing our services.</p>
</body>
</html>
";

        await emailService.SendEmailAsync(patient.User.Email, $"Invoice {invoice.InvoiceNumber}", body);
    }

    return Ok("Invoice created and emailed successfully");
}

       
[HttpGet("claims")]
public async Task<IActionResult> ViewClaims([FromQuery] PaginationParams pagination)
{
    var claims = await _claimRepository.GetClaimsWithInvoiceAsync();

    var response = _mapper.Map<IEnumerable<ClaimResponseDTO>>(claims);

    return Ok(response);
}
[HttpGet("invoice-requests")]
public async Task<IActionResult> GetInvoiceRequests()
{
    var userId   = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var provider = await _context.HealthcareProviders
        .FirstOrDefaultAsync(p => p.UserId == userId);
    if (provider == null) return NotFound("Provider not found");
 
    var requests = await _context.InvoiceRequests
    .Include(r => r.Patient)
        .ThenInclude(p => p.User)
    .Include(r => r.Provider)              // already there?
        .ThenInclude(p => p.User)          // ✅ ADD THIS
    .Where(r => r.ProviderId == provider.ProviderId)
    .ToListAsync();
 
    var result = _mapper.Map<List<InvoiceRequestResponseDTO>>(requests);
    return Ok(result);
}
 


// POST /api/HealthcareProvider/accept-request/{requestId}
    [HttpPost("accept-request/{requestId}")]
public async Task<IActionResult> AcceptRequest(int requestId)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var provider = await _context.HealthcareProviders
        .FirstOrDefaultAsync(p => p.UserId == userId);

    if (provider == null)
        return NotFound("Provider not found");

    var request = await _context.InvoiceRequests
        .FirstOrDefaultAsync(r => r.InvoiceRequestId == requestId);

    if (request == null)
        return NotFound("Request not found");

    request.Status = "Accepted";

    // 🔥 ADD THIS PART
    var exists = await _context.ProviderPatients
        .AnyAsync(x => x.ProviderId == provider.ProviderId &&
                       x.PatientId == request.PatientId);

    if (!exists)
    {
        _context.ProviderPatients.Add(new ProviderPatient
        {
            ProviderId = provider.ProviderId,
            PatientId = request.PatientId
        });
    }

    await _context.SaveChangesAsync();

    return Ok("Request accepted. Patient added to your list.");
} 
[HttpGet("my-patients")]
public async Task<IActionResult> GetMyPatients()
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var provider = await _context.HealthcareProviders
        .FirstOrDefaultAsync(p => p.UserId == userId);

    if (provider == null)
        return NotFound("Provider not found");

    var patients = await _context.ProviderPatients
        .Include(pp => pp.Patient)
            .ThenInclude(p => p.User)
        .Where(pp => pp.ProviderId == provider.ProviderId)
        .Select(pp => new
        {
            pp.Patient.PatientId,
            Name = pp.Patient.User.FullName,
            Email = pp.Patient.User.Email
        })
        .ToListAsync();

    return Ok(patients);
}

[HttpGet("my-claims")]
public async Task<IActionResult> GetMyClaims([FromQuery] PaginationParams pagination)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var provider = await _context.HealthcareProviders
        .FirstOrDefaultAsync(p => p.UserId == userId);

    if (provider == null)
        return NotFound("Provider not found");

    // ✅ FILTER CLAIMS BASED ON PROVIDER
    var claims = await _context.Claims
        .Include(c => c.Invoice)
        .Where(c => c.Invoice.ProviderId == provider.ProviderId) // 🔥 KEY LINE
        .OrderByDescending(c => c.ClaimId)
        .ToListAsync();

    var response = _mapper.Map<IEnumerable<ClaimResponseDTO>>(claims);

    return Ok(response);
}
    }
}