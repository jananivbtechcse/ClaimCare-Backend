// using ClaimCare.Data;
// using ClaimCare.Models;
// using ClaimCare.Services.Generic;
// using ClaimCare.Services.Interfaces;
// using Microsoft.EntityFrameworkCore;

// namespace ClaimCare.Services.Implementations
// {
//     public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
//     {
//         public PaymentRepository(ClaimCareDbContext context) : base(context)
//         {
//         }

//         // GET PAYMENTS BY CLAIM
//        public async Task<IEnumerable<Payment>> GetPaymentsByClaimAsync(int claimId)
// {
//     return await _context.Payments
//         .Include(p => p.Claim)   
//         .Where(p => p.ClaimId == claimId)
//         .ToListAsync();
// }

//         // GET PAYMENTS BY PATIENT
//         public async Task<IEnumerable<Payment>> GetPaymentsByPatientAsync(int patientId)
//         {
//             return await _context.Payments
//                 .Include(p => p.Claim)
//                 .Where(p => p.Claim.PatientId == patientId)
//                 .ToListAsync();
//         }

//         // GET CLAIM WITH INVOICE
//         public async Task<Claim?> GetClaimWithInvoiceAsync(int claimId)
//         {
//             return await _context.Claims
//                 .Include(c => c.Patient)
//                 .ThenInclude(p => p.User)
//                 .FirstOrDefaultAsync(c => c.ClaimId == claimId);
//         }
//     }
// }


using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ClaimCareDbContext _context;

        public PaymentRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        // Fetch claim with patient + user for email, and ClaimNumber for mapping
        public async Task<Claim?> GetClaimWithInvoiceAsync(int claimId)
        {
            return await _context.Claims
                .Include(c => c.Patient)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);
        }

        // Mark claim as Paid
        public async Task UpdateClaimAsync(Claim claim)
        {
            _context.Claims.Update(claim);
        }

        // Add new payment record
        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        // Get ALL payments — MUST include Claim so ClaimNumber is available for the DTO mapper
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Claim)   // <-- required for ClaimNumber & ClaimStatus
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        // Get payments for a specific claim
        public async Task<IEnumerable<Payment>> GetPaymentsByClaimAsync(int claimId)
        {
            return await _context.Payments
                .Include(p => p.Claim)   // <-- required for ClaimNumber
                .Where(p => p.ClaimId == claimId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}