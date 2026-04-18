using PolicyService.Application.DTOs;

namespace PolicyService.Application.Services
{
    public interface IPolicyTypeService
    {
        Task<IEnumerable<PolicyTypeDto>> GetAllAsync();
        Task<PolicyTypeDto> GetByIdAsync(Guid id);
        Task<PolicyTypeDto> CreateAsync(CreatePolicyTypeDto dto);
        Task<PolicyTypeDto> UpdateAsync(Guid id, UpdatePolicyTypeDto dto);
        Task DeleteAsync(Guid id);
    }
}
