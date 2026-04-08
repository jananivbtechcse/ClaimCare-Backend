using ClaimCare.Models;
using ClaimCare.Services.Generic;

namespace ClaimCare.Services.Interfaces
{
    public interface IClaimRepository : IGenericRepository<Claim>
    {
        Task<IEnumerable<Claim>> GetClaimsByPatientAsync(int patientId);

        Task<IEnumerable<Claim>> GetAllClaimsAsync();

        Task<Claim?> GetClaimByIdAsync(int claimId);
        Task<IEnumerable<Claim>> GetClaimsWithInvoiceAsync();

        Task UpdateClaimAsync(Claim claim);
    }
}