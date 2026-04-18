using AdminService.Application.DTOs;
using AdminService.Application.Services;
using AdminService.Domain.Entities;
using AdminService.Domain.Repositories;
using System.Text.Json;

namespace AdminService.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepo;
        private readonly IDashboardService _dashboardService;

        public ReportService(IReportRepository reportRepo, IDashboardService dashboardService)
        {
            _reportRepo = reportRepo;
            _dashboardService = dashboardService;
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var reports = await _reportRepo.GetAllAsync();
            var result = new List<ReportDto>();
            foreach (var r in reports)
                result.Add(MapToDto(r));
            return result;
        }

        public async Task<ReportDto> GetByIdAsync(Guid id)
        {
            var report = await _reportRepo.GetByIdAsync(id);
            if (report == null)
                throw new KeyNotFoundException("Report not found.");
            return MapToDto(report);
        }

        public async Task<ReportDto> GenerateAsync(Guid adminId, GenerateReportDto dto)
        {
            var dashboard = await _dashboardService.GetDashboardAsync();
            string data = JsonSerializer.Serialize(dashboard);

            var report = new Report
            {
                Title = dto.ReportType + " Report - " + DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ReportType = dto.ReportType,
                Data = data,
                GeneratedBy = adminId
            };

            await _reportRepo.AddAsync(report);
            return MapToDto(report);
        }

        private ReportDto MapToDto(Report r)
        {
            return new ReportDto
            {
                Id = r.Id,
                Title = r.Title,
                ReportType = r.ReportType,
                Data = r.Data,
                GeneratedAt = r.GeneratedAt
            };
        }
    }
}
