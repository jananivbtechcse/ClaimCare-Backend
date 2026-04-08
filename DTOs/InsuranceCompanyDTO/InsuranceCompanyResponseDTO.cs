namespace ClaimCare.DTOs.InsuranceCompanyDTO
{
    public class InsuranceCompanyResponseDTO    {
        public int InsuranceCompanyId { get; set; }

        public string CompanyName { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsActive { get; set; }
    }
}