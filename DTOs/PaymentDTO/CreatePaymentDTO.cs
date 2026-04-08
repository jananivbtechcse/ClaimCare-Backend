using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.PaymentDTO
{
    public class CreatePaymentDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required]
        public decimal PaymentAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; }   // BankTransfer / UPI / Cheque

        [Required]
        [StringLength(100)]
        public string? TransactionReference { get; set; }
    }
}