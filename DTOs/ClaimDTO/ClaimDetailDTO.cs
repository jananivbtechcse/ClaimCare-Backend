namespace ClaimCare.DTOs.ClaimDTO
{
    public class ClaimDetailDTO
    {
        public int ClaimId { get; set; }

        public string ClaimNumber { get; set; }

        public decimal ClaimAmount { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string Status { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string? RejectionReason { get; set; }

        public string PatientName { get; set; }

        public string InsurancePlanName { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalPayments { get; set; }
    }
}