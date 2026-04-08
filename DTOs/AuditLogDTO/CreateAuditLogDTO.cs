using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.AuditLogDTO
{
    public class CreateAuditLogDTO
    {
        public int? UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(100)]
        public string? EntityName { get; set; }

        public int? EntityId { get; set; }

        public string? Description { get; set; }
    }
}