// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using ClaimCare.DTOs.PaymentDTO;
// using ClaimCare.Models;
// using AutoMapper;
// using ClaimCare.Services.Interfaces;

// namespace ClaimCare.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class PaymentController : ControllerBase
//     {
//         private readonly IPaymentRepository _paymentRepository;
//         private readonly IMapper _mapper;
//          private readonly EmailService _emailService;

//         public PaymentController(
//             IPaymentRepository paymentRepository,
//             IMapper mapper,
//               EmailService emailService)
//         {
//             _paymentRepository = paymentRepository;
//             _mapper = mapper;
//              _emailService = emailService;
//         }

       
     
// [HttpPost]
// [Authorize(Roles = "InsuranceCompany,Patient")]
// public async Task<IActionResult> CreatePayment(CreatePaymentDTO dto)
// {
   
//     var claim = await _paymentRepository.GetClaimWithInvoiceAsync(dto.ClaimId);

//     if (claim == null)
//         return NotFound("Claim not found");

//     var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

//     if (role == "InsuranceCompany" && claim.Status != "Approved")
//         return BadRequest("Insurance can only pay for approved claims");

//     if (role == "Patient" && claim.Status == "Pending")
//         return BadRequest("Claim still under review");


//     var payment = _mapper.Map<Payment>(dto);

   
//     payment.TransactionReference =
//         $"{dto.PaymentType.ToUpper()}-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100,999)}";

//     payment.PaymentDate = DateTime.UtcNow;

   
//     await _paymentRepository.AddAsync(payment);
//     await _paymentRepository.SaveAsync();

//     var response = _mapper.Map<PaymentResponseDTO>(payment);

   
//     var patientEmail = claim.Patient.User.Email;
//     var patientName = claim.Patient.User.FullName ?? "Patient"; // fallback name

//     string emailBody = $@"
// <html>
// <body>
//     <h2>Payment Receipt</h2>
//     <p>Dear {patientName},</p>
//     <p>Your payment for claim <b>{claim.ClaimNumber}</b> has been successfully processed.</p>
//     <table style='border-collapse: collapse; width: 60%;'>
//         <tr>
//             <td style='border: 1px solid #ddd; padding: 8px;'>Payment Type</td>
//             <td style='border: 1px solid #ddd; padding: 8px;'>{dto.PaymentType}</td>
//         </tr>
//         <tr>
//             <td style='border: 1px solid #ddd; padding: 8px;'>Transaction Reference</td>
//             <td style='border: 1px solid #ddd; padding: 8px;'>{payment.TransactionReference}</td>
//         </tr>
//         <tr>
//             <td style='border: 1px solid #ddd; padding: 8px;'>PaymentAmount</td>
//             <td style='border: 1px solid #ddd; padding: 8px;'>{dto.PaymentAmount:C}</td>
//         </tr>
//         <tr>
//             <td style='border: 1px solid #ddd; padding: 8px;'>Payment Date</td>
//             <td style='border: 1px solid #ddd; padding: 8px;'>{payment.PaymentDate:dd/MM/yyyy HH:mm}</td>
//         </tr>
//     </table>
//     <p>Thank you for using our services.</p>
// </body>
// </html>";

//     await _emailService.SendEmailAsync(
//         patientEmail,
//         "Payment Successful - Claim Receipt",
//         emailBody
//     );

//     return Ok(response);
// }

//     [HttpGet("claim/{claimId}")]
// [Authorize(Roles = "Patient,InsuranceCompany,HealthcareProvider,Admin")]
// public async Task<IActionResult> GetPaymentsByClaim(int claimId)
// {
//     var payments = await _paymentRepository.GetPaymentsByClaimAsync(claimId);

//     var response = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

//     return Ok(response);
// }
//     }
// }




using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.DTOs.PaymentDTO;
using ClaimCare.Models;
using AutoMapper;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;

        public PaymentController(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            EmailService emailService)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        // POST: api/Payment — Create a new payment and mark claim as Paid
        [HttpPost]
        [Authorize(Roles = "InsuranceCompany,Patient")]
        public async Task<IActionResult> CreatePayment(CreatePaymentDTO dto)
        {
            var claim = await _paymentRepository.GetClaimWithInvoiceAsync(dto.ClaimId);

            if (claim == null)
                return NotFound("Claim not found");

            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (role == "InsuranceCompany" && claim.Status != "Approved")
                return BadRequest("Insurance can only pay for approved claims");

            if (role == "Patient" && claim.Status == "Pending")
                return BadRequest("Claim still under review");

            var payment = _mapper.Map<Payment>(dto);

            payment.TransactionReference =
                $"{dto.PaymentType.ToUpper()}-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";

            payment.PaymentDate = DateTime.UtcNow;

            // Save payment record to Payments table
            await _paymentRepository.AddAsync(payment);

            // Update claim status to "Paid" so it no longer appears in the approved list
            claim.Status = "Paid";
            await _paymentRepository.UpdateClaimAsync(claim);

            await _paymentRepository.SaveAsync();

            var response = _mapper.Map<PaymentResponseDTO>(payment);

            // Send email receipt to patient
            var patientEmail = claim.Patient.User.Email;
            var patientName = claim.Patient.User.FullName ?? "Patient";

            string emailBody = $@"
<html>
<body>
    <h2>Payment Receipt</h2>
    <p>Dear {patientName},</p>
    <p>Your payment for claim <b>{claim.ClaimNumber}</b> has been successfully processed.</p>
    <table style='border-collapse: collapse; width: 60%;'>
        <tr>
            <td style='border: 1px solid #ddd; padding: 8px;'>Payment Type</td>
            <td style='border: 1px solid #ddd; padding: 8px;'>{dto.PaymentType}</td>
        </tr>
        <tr>
            <td style='border: 1px solid #ddd; padding: 8px;'>Transaction Reference</td>
            <td style='border: 1px solid #ddd; padding: 8px;'>{payment.TransactionReference}</td>
        </tr>
        <tr>
            <td style='border: 1px solid #ddd; padding: 8px;'>Payment Amount</td>
            <td style='border: 1px solid #ddd; padding: 8px;'>{dto.PaymentAmount:C}</td>
        </tr>
        <tr>
            <td style='border: 1px solid #ddd; padding: 8px;'>Payment Date</td>
            <td style='border: 1px solid #ddd; padding: 8px;'>{payment.PaymentDate:dd/MM/yyyy HH:mm}</td>
        </tr>
    </table>
    <p>Thank you for using our services.</p>
</body>
</html>";

            await _emailService.SendEmailAsync(
                patientEmail,
                "Payment Successful - Claim Receipt",
                emailBody
            );

            return Ok(response);
        }

        // GET: api/Payment — Get all payments (for payment history table)
        [HttpGet]
        [Authorize(Roles = "InsuranceCompany,Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            var response = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);
            return Ok(response);
        }

        // GET: api/Payment/claim/{claimId} — Get payments for a specific claim
        [HttpGet("claim/{claimId}")]
        [Authorize(Roles = "Patient,InsuranceCompany,HealthcareProvider,Admin")]
        public async Task<IActionResult> GetPaymentsByClaim(int claimId)
        {
            var payments = await _paymentRepository.GetPaymentsByClaimAsync(claimId);
            var response = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);
            return Ok(response);
        }
    }
}