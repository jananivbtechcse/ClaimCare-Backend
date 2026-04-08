namespace ClaimCare.DTOs.PatientDTO
{
    public class CreatePatientDTO
    {
        public DateTime DateOfBirth {get;set;}
        public string Gender {get;set;}
        public string Address {get;set;}
        public string? Symptoms {get;set;}
        public string? TreatmentDetails { get; set; }

        public int? InsurancePlanId {get;set;}
    }
}