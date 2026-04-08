using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class HealthcareProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProviderId { get; set; }
      //  public int ProviderId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [StringLength(150)]
        public string HospitalName { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}