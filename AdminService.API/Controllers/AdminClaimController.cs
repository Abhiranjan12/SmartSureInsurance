using AdminService.Application.DTOs;
using AdminService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.API.Controllers
{
    [ApiController]
    [Route("api/admin/claims")]
    [Authorize(Roles = "Admin")]
    public class AdminClaimController : ControllerBase
    {
        private readonly IAdminClaimService _claimService;

        public AdminClaimController(IAdminClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claim = await _claimService.GetClaimByIdAsync(id);
            return Ok(claim);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateClaimStatusDto dto)
        {
            var updated = await _claimService.UpdateClaimStatusAsync(id, dto);
            return Ok(updated);
        }
    }
}
