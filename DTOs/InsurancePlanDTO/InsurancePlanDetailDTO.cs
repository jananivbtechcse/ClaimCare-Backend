namespace ClaimCare.DTOs.InsurancePlanDTO
{
    public class InsurancePlanDetailDto
    {
        public int InsurancePlanId { get; set; }

        public string PlanName { get; set; }

        public decimal CoverageAmount { get; set; }

        public decimal PremiumAmount { get; set; }

        public string? PlanDescription { get; set; }

        public bool IsActive { get; set; }

        public string InsuranceCompanyName { get; set; }
    }
}