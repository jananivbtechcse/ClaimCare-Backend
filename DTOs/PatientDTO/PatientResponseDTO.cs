namespace ClaimCare.DTOs.PatientDTO
{
    public class PatientResponseDTO    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string InsurancePlanName { get; set; }
        public string MobileNumber { get; set; }
    }
}