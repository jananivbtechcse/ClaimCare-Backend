using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.DTOs.InvoiceDTO;
using ClaimCare.Models;
using AutoMapper;
using System.Security.Claims;
using ClaimCare.Services.Interfaces;
using ClaimCare.Data;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ClaimCareDbContext _context;
        private readonly IMapper _mapper;

        public InvoiceController(
            IInvoiceRepository invoiceRepository,
            ClaimCareDbContext context,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _context = context;
            _mapper = mapper;
        }

       
        [HttpGet("patient")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPatientInvoices()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
                return NotFound("Patient not found");

            var invoices = await _invoiceRepository
                .GetInvoicesByPatientAsync(patient.PatientId);

            var response = _mapper.Map<List<InvoiceResponseDTO>>(invoices);

            return Ok(response);
        }

        
        [HttpGet("provider")]
        [Authorize(Roles = "HealthcareProvider")]
        public async Task<IActionResult> GetProviderInvoices()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var provider = await _context.HealthcareProviders
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (provider == null)
                return NotFound("Provider not found");

            var invoices = await _invoiceRepository
                .GetInvoicesByProviderAsync(provider.ProviderId);

            var result = _mapper.Map<List<InvoiceResponseDTO>>(invoices);

            return Ok(result);
        }
    }
}