using ClaimsService.Domain.Entities;

namespace ClaimsService.Domain.Repositories
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllAsync();
        Task<IEnumerable<Claim>> GetByCustomerIdAsync(Guid customerId);
        Task<Claim?> GetByIdAsync(Guid id);
        Task<Claim?> GetByClaimNumberAsync(string claimNumber);
        Task AddAsync(Claim claim);
        Task UpdateAsync(Claim claim);
    }
}
