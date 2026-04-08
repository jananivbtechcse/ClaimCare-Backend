using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.AuthDTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}