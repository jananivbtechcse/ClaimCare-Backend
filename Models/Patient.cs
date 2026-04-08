using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimCare.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        public string? Symptoms { get; set; }
        public string? TreatmentDetails { get; set; }

        public int? InsurancePlanId { get; set; }
        public InsurancePlan? InsurancePlan { get; set; }
    }
}