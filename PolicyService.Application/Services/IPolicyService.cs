using PolicyService.Application.DTOs;

namespace PolicyService.Application.Services
{
    public interface IPolicyService
    {
        Task<IEnumerable<PolicyDto>> GetAllAsync();
        Task<IEnumerable<PolicyDto>> GetByCustomerIdAsync(Guid customerId);
        Task<PolicyDto> GetByIdAsync(Guid id);
        Task<PolicyDto> BuyPolicyAsync(Guid customerId, BuyPolicyDto dto);
        Task<PolicyDto> UpdateStatusAsync(Guid id, UpdatePolicyStatusDto dto);
    }
}
