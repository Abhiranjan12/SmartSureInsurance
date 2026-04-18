using ClaimsService.Application.DTOs;

namespace ClaimsService.Application.Services
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimDto>> GetAllAsync();
        Task<IEnumerable<ClaimDto>> GetByCustomerIdAsync(Guid customerId);
        Task<ClaimDto> GetByIdAsync(Guid id);
        Task<ClaimDto> CreateAsync(Guid customerId, CreateClaimDto dto);
        Task<ClaimDto> SubmitAsync(Guid claimId, Guid customerId);
        Task<ClaimDto> UpdateStatusAsync(Guid claimId, UpdateClaimStatusDto dto);
    }
}
