using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Generic;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class ClaimRepository : GenericRepository<Claim>, IClaimRepository
{
    public ClaimRepository(ClaimCareDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Claim>> GetClaimsByPatientAsync(int patientId)
    {
        return await _context.Claims
            .Where(c => c.PatientId == patientId)
            .Include(c => c.Invoice)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
    {
        return await _context.Claims
            .Include(c => c.Patient)
            .Include(c => c.Invoice)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> GetClaimsWithInvoiceAsync()
    {
        return await _context.Claims
            .Include(c => c.Invoice)
            .Include(c => c.Patient)
            .ThenInclude(p => p.User)
            .ToListAsync();
    }

    public async Task<Claim?> GetClaimByIdAsync(int claimId)
    {
        return await _context.Claims
            .Include(c => c.Patient)
            .ThenInclude(p => p.User)
            .Include(c => c.Invoice)
            .FirstOrDefaultAsync(c => c.ClaimId == claimId);
    }

    public async Task UpdateClaimAsync(Claim claim)
    {
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync();
    }
}
}