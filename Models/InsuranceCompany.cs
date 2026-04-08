using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class InsuranceCompany
    {
        [Key]
        public int InsuranceCompanyId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<InsurancePlan>? InsurancePlans { get; set; }
    }
}