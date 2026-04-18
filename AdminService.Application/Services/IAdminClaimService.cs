using AdminService.Application.DTOs;

namespace AdminService.Application.Services
{
    public interface IAdminClaimService
    {
        Task<IEnumerable<ClaimSummaryDto>> GetAllClaimsAsync();
        Task<ClaimSummaryDto> GetClaimByIdAsync(Guid claimId);
        Task<ClaimSummaryDto> UpdateClaimStatusAsync(Guid claimId, UpdateClaimStatusDto dto);
    }
}
