using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Provider")]
        public int ProviderId { get; set; }
        public HealthcareProvider? Provider { get; set; }

        public decimal ConsultationFee { get; set; }
        public decimal DiagnosticTestFee { get; set; }
        public decimal DiagnosticScanFee { get; set; }
        public decimal MedicineFee { get; set; }

        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string? InvoiceStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Claim>? Claims { get; set; }
    }
}  