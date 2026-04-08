// namespace ClaimCare.DTOs.InvoiceDTO
// {
//     public class InvoiceResponseDTO
//     {
//         public int InvoiceId { get; set; }

//         public string InvoiceNumber { get; set; }

//         public string PatientName { get; set; }

//         public string ProviderName { get; set; }

//         public decimal TotalAmount { get; set; }

//         public string InvoiceStatus { get; set; }

//         public DateTime InvoiceDate { get; set; }
//     }
// }


public class InvoiceResponseDTO
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public string PatientName { get; set; }
    public string ProviderName { get; set; }
    public decimal TotalAmount { get; set; }
    public string? InvoiceStatus { get; set; }
    public DateTime InvoiceDate { get; set; }

    // Add these:
    public string PatientId { get; set; }        // or int, match your model
    public string? PatientAddress { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? ConsultationFee { get; set; }
    public decimal? DiagnosticFee { get; set; }
    public decimal? DiagnosticScanFee { get; set; }
    public decimal? MedicationFee { get; set; }
    public decimal? TaxAmount { get; set; }
}