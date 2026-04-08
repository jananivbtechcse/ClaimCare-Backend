using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class InsurancePlanRepository : IInsurancePlanRepository
    {
        private readonly ClaimCareDbContext _context;

        public InsurancePlanRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InsurancePlan>> GetAllActivePlans()
        {
            return await _context.InsurancePlans
                .Include(p => p.InsuranceCompany)
                .Where(p => p.IsActive == true)
                .OrderBy(p => p.InsurancePlanId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsurancePlan>> GetPlansByCompany(int companyId)
        {
            return await _context.InsurancePlans
                .Include(p => p.InsuranceCompany)
                .Where(p => p.IsActive == true && p.InsuranceCompanyId == companyId)
                .OrderBy(p => p.InsurancePlanId)
                .ToListAsync();
        }

    
            public async Task<InsurancePlan> GetPlanById(int id)
{
    return await _context.InsurancePlans
        .Include(p => p.InsuranceCompany) // <-- important!
        .FirstOrDefaultAsync(p => p.InsurancePlanId == id);
}
        public async Task<InsuranceCompany?> GetCompanyByUserId(int userId)
        {
            return await _context.InsuranceCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddPlan(InsurancePlan plan)
        {
            await _context.InsurancePlans.AddAsync(plan);
        }

        public async Task UpdatePlan(InsurancePlan plan)
        {
            _context.InsurancePlans.Update(plan);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}