using System;

namespace ClaimCare.DTOs
{
    public class InvoiceRequestResponseDTO
    {
        public int InvoiceRequestId { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";

        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = "";

        public DateTime VisitDate { get; set; }

        public string Status { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}