namespace ClaimCare.DTOs.PatientDTO
{
    public class PatientDetailDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string? Symptoms { get; set; }

        public string? TreatmentDetails { get; set; }

        public string? InsurancePlanName { get; set; }
    }
}