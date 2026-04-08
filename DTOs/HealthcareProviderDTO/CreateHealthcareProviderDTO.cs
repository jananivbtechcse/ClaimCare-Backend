using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.HealthcareProviderDTO
{
    public class CreateHealthcareProviderDTO
{
    [Required]
    [StringLength(150)]
    public string HospitalName { get; set; } 

    [Required]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;
}
}