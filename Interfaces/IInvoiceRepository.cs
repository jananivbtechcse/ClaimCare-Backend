using ClaimCare.Models;
using ClaimCare.Services.Generic;

namespace ClaimCare.Services.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetInvoicesByProviderAsync(int providerId);
        Task<IEnumerable<Invoice>> GetInvoicesByPatientAsync(int patientId);
    }
}