using ClaimsService.Domain.Entities;
using ClaimsService.Domain.Repositories;
using ClaimsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClaimsService.Infrastructure.Repositories
{
    public class ClaimDocumentRepository : IClaimDocumentRepository
    {
        private readonly ClaimsDbContext _context;

        public ClaimDocumentRepository(ClaimsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClaimDocument>> GetByClaimIdAsync(Guid claimId)
        {
            return await _context.ClaimDocuments
                .Where(d => d.ClaimId == claimId)
                .ToListAsync();
        }

        public async Task<ClaimDocument?> GetByIdAsync(Guid id)
        {
            return await _context.ClaimDocuments.FindAsync(id);
        }

        public async Task AddAsync(ClaimDocument document)
        {
            await _context.ClaimDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClaimDocument document)
        {
            _context.ClaimDocuments.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ClaimDocument document)
        {
            _context.ClaimDocuments.Remove(document);
            await _context.SaveChangesAsync();
        }
    }
}
