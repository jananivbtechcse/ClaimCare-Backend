namespace ClaimCare.DTOs.UserDTO
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }     // better match with Entity

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}