using ClaimsService.Domain.Entities;
using ClaimsService.Domain.Repositories;
using ClaimsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClaimsService.Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly ClaimsDbContext _context;

        public ClaimRepository(ClaimsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetAllAsync()
        {
            return await _context.Claims
                .Include(c => c.Documents)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Claims
                .Include(c => c.Documents)
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Claim?> GetByIdAsync(Guid id)
        {
            return await _context.Claims
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Claim?> GetByClaimNumberAsync(string claimNumber)
        {
            return await _context.Claims
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.ClaimNumber == claimNumber);
        }

        public async Task AddAsync(Claim claim)
        {
            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Claim claim)
        {
            claim.UpdatedAt = DateTime.UtcNow;
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
        }
    }
}
