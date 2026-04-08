namespace ClaimCare.DTOs.ClaimDocumentDTO
{
    public class ClaimDocumentDetailDTO
    {
        public int ClaimDocumentId { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }  // Safe public URL

        public DateTime UploadedDate { get; set; }

        public int ClaimId { get; set; }
    }
}