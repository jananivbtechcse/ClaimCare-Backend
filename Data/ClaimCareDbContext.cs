using Microsoft.EntityFrameworkCore;
using ClaimCare.Models;

namespace ClaimCare.Data
{
    public class ClaimCareDbContext : DbContext
    {
        public ClaimCareDbContext(DbContextOptions<ClaimCareDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<HealthcareProvider> HealthcareProviders { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<InvoiceRequest> InvoiceRequests { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ProviderPatient> ProviderPatients { get; set; }
        public DbSet<ClaimDocument> ClaimDocuments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. ONE-TO-ONE RELATIONSHIPS ---
            // User and Patient
            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User and HealthcareProvider
            modelBuilder.Entity<User>()
                .HasOne(u => u.HealthcareProvider)
                .WithOne(hp => hp.User)
                .HasForeignKey<HealthcareProvider>(hp => hp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User and InsuranceCompany
            modelBuilder.Entity<User>()
                .HasOne(u => u.InsuranceCompany)
                .WithOne(ic => ic.User)
                .HasForeignKey<InsuranceCompany>(ic => ic.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- 2. ONE-TO-MANY RELATIONSHIPS ---
            // Role and Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // InsuranceCompany and InsurancePlans
            modelBuilder.Entity<InsurancePlan>()
                .HasOne(ip => ip.InsuranceCompany)
                .WithMany(ic => ic.InsurancePlans)
                .HasForeignKey(ip => ip.InsuranceCompanyId);

            // Patient and Invoices
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Patient)
                .WithMany()
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthcareProvider and Invoices
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Provider)
                .WithMany(hp => hp.Invoices)
                .HasForeignKey(i => i.ProviderId);

            // Claim Relationships
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Invoice)
                .WithMany(i => i.Claims)
                .HasForeignKey(c => c.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.InsurancePlan)
                .WithMany(ip => ip.Claims)
                .HasForeignKey(c => c.InsurancePlanId);

            // Claim and Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Claim)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.ClaimId);

            // Claim and ClaimDocuments
            modelBuilder.Entity<ClaimDocument>()
                .HasOne(cd => cd.Claim)
                .WithMany(c => c.ClaimDocuments)
                .HasForeignKey(cd => cd.ClaimId);

            // User and Notifications
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            // User and AuditLogs
            modelBuilder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // --- 3. PROPERTY CONFIGURATIONS (Precision & Defaults) ---
            // Decimal precision for financial fields
            modelBuilder.Entity<Claim>(entity => {
                entity.Property(e => e.ClaimAmount).HasPrecision(18, 2);
                entity.Property(e => e.InvoiceAmount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<InsurancePlan>(entity => {
                entity.Property(e => e.CoverageAmount).HasPrecision(18, 2);
                entity.Property(e => e.PremiumAmount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Invoice>(entity => {
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
                entity.Property(e => e.ConsultationFee).HasPrecision(18, 2);
                entity.Property(e => e.DiagnosticTestFee).HasPrecision(18, 2);
                entity.Property(e => e.DiagnosticScanFee).HasPrecision(18, 2);
                entity.Property(e => e.MedicineFee).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentAmount).HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceRequest>()
    .HasOne(ir => ir.Patient)
    .WithMany()
    .HasForeignKey(ir => ir.PatientId)
    .OnDelete(DeleteBehavior.NoAction);

modelBuilder.Entity<InvoiceRequest>()
    .HasOne(ir => ir.Provider)
    .WithMany()
    .HasForeignKey(ir => ir.ProviderId)
    .OnDelete(DeleteBehavior.NoAction);


     modelBuilder.Entity<ProviderPatient>()
        .HasOne(pp => pp.Patient)
        .WithMany()
        .HasForeignKey(pp => pp.PatientId)
        .OnDelete(DeleteBehavior.NoAction);   // ✅ FIX

    modelBuilder.Entity<ProviderPatient>()
        .HasOne(pp => pp.Provider)
        .WithMany()
        .HasForeignKey(pp => pp.ProviderId)
        .OnDelete(DeleteBehavior.NoAction);   // ✅ FIX
        }


        
    }
}