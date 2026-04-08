using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.InsurancePlanDTO
{
    public class UpdateInsurancePlanDto
    {
        public string? PlanName { get; set; }

        public decimal? CoverageAmount { get; set; }

        public decimal? PremiumAmount { get; set; }

        public string? PlanDescription { get; set; }

        public bool? IsActive { get; set; }
    }
}