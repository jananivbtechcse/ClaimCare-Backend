namespace ClaimCare.DTOs.ClaimDocumentDTO
{
    public class ClaimDocumentResponseDTO
    {
        public int ClaimDocumentId { get; set; }

        public string FileName { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}