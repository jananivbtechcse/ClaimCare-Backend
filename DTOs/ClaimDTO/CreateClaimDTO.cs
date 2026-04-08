using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.ClaimDTO
{
    public class CreateClaimDTO
    {
       

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int InsurancePlanId { get; set; }

        [Required]
        public decimal ClaimAmount { get; set; }

        [Required]
        public decimal InvoiceAmount { get; set; }
    }
}