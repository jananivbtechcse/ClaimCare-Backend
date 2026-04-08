using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ClaimCare.DTOs.ClaimDocumentDTO
{
    public class UploadClaimDocumentDTO
    {
        [Required]
        public int ClaimId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}