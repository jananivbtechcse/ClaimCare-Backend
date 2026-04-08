// namespace ClaimCare.DTOs.PaymentDTO
// {
//     public class PaymentResponseDTO    {
//         public int PaymentId { get; set; }

//         public decimal PaymentAmount { get; set; }

//         public DateTime PaymentDate { get; set; }

//         public string PaymentType { get; set; }

//         public string ClaimStatus { get; set; }
//     }
// }


namespace ClaimCare.DTOs.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public int PaymentId { get; set; }
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; }          // from Claim.ClaimNumber
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string TransactionReference { get; set; } // from Payment.TransactionReference
        public string ClaimStatus { get; set; }
    }
}