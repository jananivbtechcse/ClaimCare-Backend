using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class ClaimDocument
    {
        public int ClaimDocumentId {get;set;}

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }
        public Claim? Claim { get; set; }


        [Required]
        [StringLength(100)]
        public string FileName { get; set; }
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

    }
}