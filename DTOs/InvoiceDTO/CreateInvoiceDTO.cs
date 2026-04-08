namespace ClaimCare.DTOs.InvoiceDTO
{
    public class CreateInvoiceDTO
    {
        public int PatientId { get; set; }

        public int ProviderId { get; set; }

        public decimal ConsultationFee { get; set; }

        public decimal DiagnosticTestFee { get; set; }

        public decimal DiagnosticScanFee { get; set; }

        public decimal MedicineFee { get; set; }

        public decimal TaxPercentage { get; set; }
    }
}