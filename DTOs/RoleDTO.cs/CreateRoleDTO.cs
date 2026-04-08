using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.RoleDTO
{
    public class CreateRoleDTO
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}