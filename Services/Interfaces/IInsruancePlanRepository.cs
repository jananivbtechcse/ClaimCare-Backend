using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface IInsurancePlanRepository
    {
        Task<IEnumerable<InsurancePlan>> GetAllActivePlans();

        Task<InsurancePlan?> GetPlanById(int id);

        Task<IEnumerable<InsurancePlan>> GetPlansByCompany(int companyId);

        Task<InsuranceCompany?> GetCompanyByUserId(int userId);

        Task AddPlan(InsurancePlan plan);

        Task UpdatePlan(InsurancePlan plan);

        Task SaveAsync();
    }
}