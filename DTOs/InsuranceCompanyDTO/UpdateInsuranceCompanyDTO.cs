using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.InsuranceCompanyDTO
{
    public class UpdateInsuranceCompanyDTO    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string? RegistrationNumber {get;set;}

        public bool? IsActive { get; set; }   // Admin control
    }
}