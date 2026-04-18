using Microsoft.EntityFrameworkCore;
using PolicyService.Domain.Entities;
using PolicyService.Domain.Repositories;
using PolicyService.Infrastructure.Data;

namespace PolicyService.Infrastructure.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly PolicyDbContext _context;

        public PolicyRepository(PolicyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            return await _context.Policies
                .Include(p => p.PolicyType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Policy>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Policies
                .Include(p => p.PolicyType)
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Policy?> GetByIdAsync(Guid id)
        {
            return await _context.Policies
                .Include(p => p.PolicyType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Policy?> GetByPolicyNumberAsync(string policyNumber)
        {
            return await _context.Policies
                .Include(p => p.PolicyType)
                .FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
        }

        public async Task AddAsync(Policy policy)
        {
            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Policy policy)
        {
            policy.UpdatedAt = DateTime.UtcNow;
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();
        }
    }
}
