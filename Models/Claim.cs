using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClaimNumber { get; set; }

        // Patient
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        // Invoice
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        // Insurance Plan
        [ForeignKey("InsurancePlan")]
        public int InsurancePlanId { get; set; }
        public InsurancePlan? InsurancePlan { get; set; }

        [Required]
        public decimal ClaimAmount { get; set; }

        [Required]
        public decimal InvoiceAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; 
        // Pending / Approved / Rejected

        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedDate { get; set; }

        public string? RejectionReason { get; set; }

        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ClaimDocument>? ClaimDocuments { get; set; }
    }
}