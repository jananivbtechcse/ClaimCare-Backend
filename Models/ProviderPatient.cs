namespace ClaimCare.Models{
public class ProviderPatient
{
    public int Id { get; set; }

    public int ProviderId { get; set; }
    public HealthcareProvider Provider { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

}