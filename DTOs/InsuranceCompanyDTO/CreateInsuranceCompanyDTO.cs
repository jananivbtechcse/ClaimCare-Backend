using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.InsuranceCompanyDTO
{
    public class CreateInsuranceCompanyDTO    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}