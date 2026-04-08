using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface IInsuranceCompanyRepository
    {
        Task<InsuranceCompany?> GetCompanyByUserId(int userId);

        Task UpdateCompany(InsuranceCompany company);
        Task<List<InsurancePlan>> GetPlansByCompanyId(int companyId);

        Task<IEnumerable<Claim>> GetAllClaims();

        Task<Claim?> GetClaimById(int id);

        Task<IEnumerable<InsurancePlan>> GetAllPlans();

        Task SaveAsync();
    }
}