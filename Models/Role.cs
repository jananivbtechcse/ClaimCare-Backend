using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimCare.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}