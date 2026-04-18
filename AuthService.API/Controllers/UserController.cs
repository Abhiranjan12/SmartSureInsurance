using AuthService.Application.DTOs;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Customer — apna profile dekhe
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            Guid userId = GetCurrentUserId();
            var user = await _userService.GetByIdAsync(userId);
            return Ok(user);
        }

        // Customer — apna profile update kare (sirf FullName)
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            Guid userId = GetCurrentUserId();
            var updated = await _userService.UpdateProfileAsync(userId, dto);
            return Ok(updated);
        }

        // Admin — saare users ki list
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // Admin — ek user ki detail
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        // Admin — user update (naam + role)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var updated = await _userService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        // Admin — user delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? User.FindFirstValue("sub");

            if (userIdStr == null)
                throw new UnauthorizedAccessException("User identity not found.");

            return Guid.Parse(userIdStr);
        }
    }
}
