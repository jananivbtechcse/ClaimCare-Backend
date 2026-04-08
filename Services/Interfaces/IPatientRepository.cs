using ClaimCare.Models;
using ClaimCare.Services.Generic;

namespace ClaimCare.Services.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetPatientWithUserAsync(int userId);

        Task<Patient?> GetPatientWithDetailsAsync(int patientId);

        Task UpdatePatientAsync(Patient patient);

        Task<Patient?> GetPatientByUserIdAsync(int userId);
    }
}