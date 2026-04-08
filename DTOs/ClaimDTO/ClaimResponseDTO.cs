// namespace ClaimCare.DTOs.ClaimDTO
// {
//     public class ClaimResponseDTO
//     {
        
//         public int ClaimId { get; set; }

//         public string ClaimNumber { get; set; }
//         public int PatientId;
//         public int InvoiceId;

//         public decimal ClaimAmount { get; set; }

//         public string Status { get; set; }

//         public DateTime SubmissionDate { get; set; }

//         public string InvoiceNumber { get; set; }

//         public decimal TotalAmount { get; set; }
//     }
// }


public class ClaimResponseDTO
{
    public int ClaimId { get; set; }
    public string ClaimNumber { get; set; }
    public int PatientId { get; set; }   // ← add { get; set; }
    public int InvoiceId { get; set; }   // ← add { get; set; }
    public decimal ClaimAmount { get; set; }
    public string Status { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string InvoiceNumber { get; set; }
    public decimal TotalAmount { get; set; }
}