using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class InsurancePlan
    {
        [Key]
        public int? InsurancePlanId { get; set; }

        [ForeignKey("InsuranceCompany")]
        public int InsuranceCompanyId { get; set; }
        public InsuranceCompany? InsuranceCompany { get; set; }

        [Required]
        public string PlanName { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        public string? PlanDescription { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Claim>? Claims { get; set; }
    }
}