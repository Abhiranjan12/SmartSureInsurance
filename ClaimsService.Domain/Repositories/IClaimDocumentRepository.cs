using ClaimsService.Domain.Entities;

namespace ClaimsService.Domain.Repositories
{
    public interface IClaimDocumentRepository
    {
        Task<IEnumerable<ClaimDocument>> GetByClaimIdAsync(Guid claimId);
        Task<ClaimDocument?> GetByIdAsync(Guid id);
        Task AddAsync(ClaimDocument document);
        Task UpdateAsync(ClaimDocument document);
        Task DeleteAsync(ClaimDocument document);
    }
}
