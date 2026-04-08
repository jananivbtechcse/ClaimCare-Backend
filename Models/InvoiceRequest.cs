using System;
using ClaimCare.Models;   // ✅ adjust if your namespace differs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClaimCare.Models{
public class InvoiceRequest
{
    public int InvoiceRequestId { get; set; }
    public int PatientId        { get; set; }
    public int ProviderId       { get; set; }
    public DateTime VisitDate   { get; set; }

    // "Pending" | "Accepted" | "Rejected"
    public string Status        { get; set; } = "Pending";
    public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;
 
    // Navigation
    public Patient Patient  { get; set; } = null!;
    public HealthcareProvider Provider { get; set; } = null!;
}
}