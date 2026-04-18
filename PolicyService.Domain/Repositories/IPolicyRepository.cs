using PolicyService.Domain.Entities;

namespace PolicyService.Domain.Repositories
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<Policy>> GetAllAsync();
        Task<IEnumerable<Policy>> GetByCustomerIdAsync(Guid customerId);
        Task<Policy?> GetByIdAsync(Guid id);
        Task<Policy?> GetByPolicyNumberAsync(string policyNumber);
        Task AddAsync(Policy policy);
        Task UpdateAsync(Policy policy);
    }
}
