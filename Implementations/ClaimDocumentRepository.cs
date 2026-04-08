 using ClaimCare.Data;
using ClaimCare.Models;
using ClaimCare.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimCare.Services.Implementations
{
    public class ClaimDocumentRepository : IClaimDocumentRepository
    {
        private readonly ClaimCareDbContext _context;

        public ClaimDocumentRepository(ClaimCareDbContext context)
        {
            _context = context;
        }

        public async Task AddDocument(ClaimDocument document)
        {
            await _context.ClaimDocuments.AddAsync(document);
        }

        public async Task<IEnumerable<ClaimDocument>> GetDocumentsByClaimId(int claimId)
        {
            return await _context.ClaimDocuments
                .Where(d => d.ClaimId == claimId)
                .ToListAsync();
        }

        public async Task<ClaimDocument?> GetDocumentById(int id)
        {
            return await _context.ClaimDocuments
                .FirstOrDefaultAsync(d => d.ClaimDocumentId == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}