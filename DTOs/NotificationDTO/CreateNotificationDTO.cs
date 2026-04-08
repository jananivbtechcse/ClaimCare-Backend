using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.NotificationDTO
{
    public class CreateNotificationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
}