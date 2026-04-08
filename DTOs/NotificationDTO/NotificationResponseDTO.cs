namespace ClaimCare.DTOs.NotificationDTO
{
    public class NotificationResponseDTO
    {
        public int NotificationId { get; set; }

        public string? Message { get; set; } = string.Empty;

        public bool IsEmailSent { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}