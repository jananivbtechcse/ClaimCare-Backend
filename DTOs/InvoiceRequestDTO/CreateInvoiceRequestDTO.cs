using System;

namespace ClaimCare.DTOs
{
    public class CreateInvoiceRequestDTO
    {
        public int ProviderId { get; set; }
        public DateTime VisitDate { get; set; }
       
    }
}