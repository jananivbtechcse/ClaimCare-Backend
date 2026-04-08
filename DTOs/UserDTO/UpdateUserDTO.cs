namespace ClaimCare.DTOs.UserDTO
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }   // optional — only hashed if provided
        public bool? IsActive { get; set; }      // optional — toggle account on/off
    }
}