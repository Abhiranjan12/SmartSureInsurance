using PolicyService.Application.DTOs;

namespace PolicyService.Application.Services
{
    public interface IPremiumService
    {
        Task<PremiumResultDto> CalculateAsync(CalculatePremiumDto dto);
    }
}
