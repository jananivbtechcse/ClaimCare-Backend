using System.ComponentModel.DataAnnotations;

namespace ClaimCare.DTOs.InvoiceDTO
{
    public class UpdateInvoiceDTO
    {
        public decimal ConsultationFee { get; set; }

        public decimal DiagnosticTestFee { get; set; }

        public decimal DiagnosticScanFee { get; set; }

        public decimal MedicineFee { get; set; }

        public decimal TaxPercentage { get; set; }

        public string? InvoiceStatus { get; set; }
    }
}