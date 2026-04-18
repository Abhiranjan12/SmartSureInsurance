using PolicyService.Domain.Entities;

namespace PolicyService.Domain.Repositories
{
    public interface IPolicyTypeRepository
    {
        Task<IEnumerable<PolicyType>> GetAllAsync();
        Task<PolicyType?> GetByIdAsync(Guid id);
        Task AddAsync(PolicyType policyType);
        Task UpdateAsync(PolicyType policyType);
        Task DeleteAsync(PolicyType policyType);
    }
}
