// using ClaimCare.Models;
// using ClaimCare.Services.Generic;

// namespace ClaimCare.Services.Interfaces
// {
//     public interface IPaymentRepository : IGenericRepository<Payment>
//     {
//         Task<IEnumerable<Payment>> GetPaymentsByClaimAsync(int claimId);

//         Task<IEnumerable<Payment>> GetPaymentsByPatientAsync(int patientId);

//         Task<Claim?> GetClaimWithInvoiceAsync(int claimId);
//     }
// }


using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface IPaymentRepository
    {
        // Claim helpers
        Task<Claim?> GetClaimWithInvoiceAsync(int claimId);
        Task UpdateClaimAsync(Claim claim);

        // Payment CRUD
        Task AddAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByClaimAsync(int claimId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();

        Task SaveAsync();
    }
}