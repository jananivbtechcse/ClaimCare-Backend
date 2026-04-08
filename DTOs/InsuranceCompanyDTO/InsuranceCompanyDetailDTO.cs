namespace ClaimCare.DTOs.InsuranceCompanyDTO
{
    public class InsuranceCompanyDetailDTO
    {
        public int InsuranceCompanyId { get; set; }

        public string CompanyName { get; set; }

        public string RegistrationNumber { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }   // from User table

        public int TotalPlans { get; set; } // count only, not full list
    }
}