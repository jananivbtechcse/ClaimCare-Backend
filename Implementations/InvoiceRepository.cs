using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Generic;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ClaimCareDbContext context) : base(context)
        {
        }

        // GET INVOICES BY PROVIDER
        public async Task<IEnumerable<Invoice>> GetInvoicesByProviderAsync(int providerId)
        {
            return await _context.Invoices
                .Where(i => i.ProviderId == providerId)
                .Include(i => i.Patient)
                    .ThenInclude(p => p.User)
                .Include(i => i.Provider)
                .ToListAsync();
        }

        // GET INVOICES BY PATIENT
        public async Task<IEnumerable<Invoice>> GetInvoicesByPatientAsync(int patientId)
        {
            return await _context.Invoices
                .Where(i => i.PatientId == patientId)
                .Include(i => i.Patient)
                    .ThenInclude(p => p.User)
                .Include(i => i.Provider)
                .ToListAsync();
        }
    }
}