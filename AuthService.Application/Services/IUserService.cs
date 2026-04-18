using AuthService.Application.DTOs;

namespace AuthService.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto);
        Task<UserDto> UpdateProfileAsync(Guid id, UpdateProfileDto dto);
        Task DeleteAsync(Guid id);
    }
}
