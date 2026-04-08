namespace ClaimCare.DTOs.AuditLogDTO
{
    public class AuditLogDetailDTO
    {
        public int AuditLogId { get; set; }

        public string Action { get; set; }

        public string? EntityName { get; set; }

        public int? EntityId { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Description { get; set; }

        public string? UserEmail { get; set; }
    }
}