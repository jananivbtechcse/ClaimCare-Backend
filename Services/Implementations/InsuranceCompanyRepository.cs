using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class InsuranceCompanyRepository : IInsuranceCompanyRepository
    {
        private readonly ClaimCareDbContext _context;

        public InsuranceCompanyRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        public async Task<InsuranceCompany?> GetCompanyByUserId(int userId)
        {
            return await _context.InsuranceCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpdateCompany(InsuranceCompany company)
        {
            _context.InsuranceCompanies.Update(company);
        }

        public async Task<IEnumerable<Claim>> GetAllClaims()
        {
            return await _context.Claims
                .Include(c => c.Patient)
                .Include(c => c.Invoice)
                .ToListAsync();
        }

        public async Task<Claim?> GetClaimById(int id)
        {
            return await _context.Claims.FindAsync(id);
        }

        public async Task<IEnumerable<InsurancePlan>> GetAllPlans()
        {
            return await _context.InsurancePlans
                .Include(p => p.InsuranceCompany)
                .ToListAsync();
        }
        public async Task<List<InsurancePlan>> GetPlansByCompanyId(int companyId)
            {
                return await _context.InsurancePlans
                    .Where(p => p.InsuranceCompanyId == companyId)
                    .ToListAsync();
            }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}