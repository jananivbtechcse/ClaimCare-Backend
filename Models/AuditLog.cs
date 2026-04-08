using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }
        // Created Claim / Approved Claim / Login / Updated Profile

        [StringLength(100)]
        public string? EntityName { get; set; }
        // Claim / Invoice / User

        public int? EntityId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? Description { get; set; }
    }
}