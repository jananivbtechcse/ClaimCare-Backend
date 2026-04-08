using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface IHealthcareProviderRepository
    {
        Task<IEnumerable<HealthcareProvider>> GetAllProviders();
        Task<HealthcareProvider> GetProviderById(int id);
        Task<HealthcareProvider> CreateProvider(HealthcareProvider provider);
        Task UpdateProvider(HealthcareProvider provider);
        Task DeleteProvider(int id);
    }
}