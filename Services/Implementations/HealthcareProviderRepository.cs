using Microsoft.EntityFrameworkCore;
using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;

namespace ClaimCare.Services.Implementations
{
    public class HealthcareProviderRepository : IHealthcareProviderRepository
    {
        private readonly ClaimCareDbContext _context;

        public HealthcareProviderRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HealthcareProvider>> GetAllProviders()
        {
            return await _context.HealthcareProviders
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<HealthcareProvider> GetProviderById(int id)
        {
             return await _context.HealthcareProviders
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProviderId == id);
        }

        public async Task<HealthcareProvider> CreateProvider(HealthcareProvider provider)
        {
           await _context.HealthcareProviders.AddAsync(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task UpdateProvider(HealthcareProvider provider)
        {
            _context.HealthcareProviders.Update(provider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProvider(int id)
        {
            var provider = await _context.HealthcareProviders.FindAsync(id);
            if (provider != null)
            {
                _context.HealthcareProviders.Remove(provider);
                await _context.SaveChangesAsync();
            }
        }
    }
}