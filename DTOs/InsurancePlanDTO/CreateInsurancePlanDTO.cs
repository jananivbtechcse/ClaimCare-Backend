using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.InsurancePlanDTO
{
    public class CreateInsurancePlanDTO
    {
        [Required]
        public int InsuranceCompanyId {get;set;}


        [Required]
        public string PlanName { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        public string? PlanDescription { get; set; }

    }
}