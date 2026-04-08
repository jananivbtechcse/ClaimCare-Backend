using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.AuthDTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}