
namespace ClaimCare.DTOs.ClaimDTO
{
public class ClaimStatusResponseDTO
{
    public int ClaimId { get; set; }
    public string Status { get; set; }
    public string RejectionReason { get; set; }
}
}