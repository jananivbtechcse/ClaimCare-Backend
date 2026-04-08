using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }
        public Claim? Claim { get; set; }

        [Required]
        public decimal PaymentAmount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; } 
        // BankTransfer / UPI / Cheque

        [Required]
        [StringLength(100)]
        public string TransactionReference { get; set; }
    }
}