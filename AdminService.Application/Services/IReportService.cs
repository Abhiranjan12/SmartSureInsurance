using AdminService.Application.DTOs;

namespace AdminService.Application.Services
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllAsync();
        Task<ReportDto> GetByIdAsync(Guid id);
        Task<ReportDto> GenerateAsync(Guid adminId, GenerateReportDto dto);
    }
}
