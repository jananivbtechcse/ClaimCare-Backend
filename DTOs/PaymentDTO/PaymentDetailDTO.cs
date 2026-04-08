namespace ClaimCare.DTOs.PaymentDTO
{
    public class PaymentDetailDTO
    {
        public int PaymentId { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentType { get; set; }

        public string TransactionReference { get; set; }

        public int ClaimId { get; set; }

        public decimal ClaimAmount { get; set; }
    }
}