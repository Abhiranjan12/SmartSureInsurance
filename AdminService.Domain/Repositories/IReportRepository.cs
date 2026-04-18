using AdminService.Domain.Entities;

namespace AdminService.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid id);
        Task AddAsync(Report report);
    }
}
