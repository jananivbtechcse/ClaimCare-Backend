using ClaimCare.Models;

namespace ClaimCare.Services.Interfaces
{
    public interface IClaimDocumentRepository
    {
        Task AddDocument(ClaimDocument document);

        Task<IEnumerable<ClaimDocument>> GetDocumentsByClaimId(int claimId);

        Task<ClaimDocument?> GetDocumentById(int id);

        Task SaveAsync();
    }
}