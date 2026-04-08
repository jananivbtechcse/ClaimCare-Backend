using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class Notification
    {
        public int NotificationId {get;set;}

        [ForeignKey("User")]
        public int UserId {get;set;}
        public User? User {get;set;}

        [Required]
        [StringLength(500)]
        public string Message {get;set;}

        public bool IsEmailSent {get;set;} = false;

        public DateTime CreatedAt {get;set;} =DateTime.UtcNow;
    }
}