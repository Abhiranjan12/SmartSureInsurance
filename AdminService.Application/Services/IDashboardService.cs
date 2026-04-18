using AdminService.Application.DTOs;

namespace AdminService.Application.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync();
    }
}
