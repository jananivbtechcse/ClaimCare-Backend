namespace ClaimCare.DTOs.InvoiceDTO
{
    public class InvoiceDetailDTO
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime DueDate { get; set; }

        public string PatientName { get; set; }

        public string ProviderName { get; set; }

        public decimal ConsultationFee { get; set; }

        public decimal DiagnosticTestFee { get; set; }

        public decimal DiagnosticScanFee { get; set; }

        public decimal MedicineFee { get; set; }

        public decimal TaxPercentage { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string InvoiceStatus { get; set; }
    }
}