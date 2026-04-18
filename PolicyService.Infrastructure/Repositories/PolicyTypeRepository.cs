using Microsoft.EntityFrameworkCore;
using PolicyService.Domain.Entities;
using PolicyService.Domain.Repositories;
using PolicyService.Infrastructure.Data;

namespace PolicyService.Infrastructure.Repositories
{
    public class PolicyTypeRepository : IPolicyTypeRepository
    {
        private readonly PolicyDbContext _context;

        public PolicyTypeRepository(PolicyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PolicyType>> GetAllAsync()
        {
            return await _context.PolicyTypes.ToListAsync();
        }

        public async Task<PolicyType?> GetByIdAsync(Guid id)
        {
            return await _context.PolicyTypes.FindAsync(id);
        }

        public async Task AddAsync(PolicyType policyType)
        {
            await _context.PolicyTypes.AddAsync(policyType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PolicyType policyType)
        {
            _context.PolicyTypes.Update(policyType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PolicyType policyType)
        {
            _context.PolicyTypes.Remove(policyType);
            await _context.SaveChangesAsync();
        }
    }
}
