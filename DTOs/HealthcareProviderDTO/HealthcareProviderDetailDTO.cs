namespace ClaimCare.DTOs.HealthcareProviderDTO
{
    public class HealthcareProviderDetailDto
    {
        public int ProviderId { get; set; }

        public string HospitalName { get; set; }

        public string LicenseNumber { get; set; }

        public string Address { get; set; }

        public string FullName { get; set; }   // from User

        public string Email { get; set; }

        public int InvoiceCount { get; set; }  // optional
    }
}