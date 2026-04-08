using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Generic;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ClaimCareDbContext context) : base(context)
        {
        }

        public async Task<Patient?> GetPatientWithUserAsync(int userId)
        {
            return await _context.Patients
        .Include(p => p.User)
        .Include(p => p.InsurancePlan)  
        .FirstOrDefaultAsync(p => p.UserId == userId);
        }

       
        public async Task<Patient?> GetPatientWithDetailsAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.InsurancePlan)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

       
        public async Task<Patient?> GetPatientByUserIdAsync(int userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        
            public async Task UpdatePatientAsync(Patient patient)
            {
                var existing = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);

                if (existing == null) return;

               
                if (patient.DateOfBirth != default && patient.DateOfBirth != DateTime.MinValue) 
                    existing.DateOfBirth = patient.DateOfBirth;

                if (!string.IsNullOrEmpty(patient.Gender)) 
                    existing.Gender = patient.Gender;

                if (!string.IsNullOrEmpty(patient.Address)) 
                    existing.Address = patient.Address;

                if (!string.IsNullOrEmpty(patient.Symptoms)) 
                    existing.Symptoms = patient.Symptoms;

                if (!string.IsNullOrEmpty(patient.TreatmentDetails)) 
                    existing.TreatmentDetails = patient.TreatmentDetails;

                if (patient.InsurancePlanId.HasValue)
                    existing.InsurancePlanId = patient.InsurancePlanId.Value;

                await _context.SaveChangesAsync();
            }
    }
}