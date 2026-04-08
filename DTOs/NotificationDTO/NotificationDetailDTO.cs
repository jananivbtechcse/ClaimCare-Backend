namespace ClaimCare.DTOs.NotificationDTO
{
    public class NotificationDetailDTO
    {
        public int NotificationId { get; set; }

        public string Message { get; set; }

        public bool IsEmailSent { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserEmail { get; set; }  // from User table
    }
}