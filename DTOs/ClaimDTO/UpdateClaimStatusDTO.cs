using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.ClaimDTO
{
    public class UpdateClaimStatusDTO
    {
        [Required]
        [StringLength(20)]
        public string Status { get; set; } 
        // Approved / Rejected

        public string? RejectionReason { get; set; }
    }
}