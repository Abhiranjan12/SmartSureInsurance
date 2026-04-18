using ClaimsService.Application.DTOs;
using ClaimsService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClaimsService.API.Controllers
{
    [ApiController]
    [Route("api/claims")]
    [Authorize]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var claims = await _claimService.GetAllAsync();
            return Ok(claims);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyClaims()
        {
            Guid customerId = GetCurrentUserId();
            var claims = await _claimService.GetByCustomerIdAsync(customerId);
            return Ok(claims);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claim = await _claimService.GetByIdAsync(id);
            return Ok(claim);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClaimDto dto)
        {
            Guid customerId = GetCurrentUserId();
            var claim = await _claimService.CreateAsync(customerId, dto);
            return CreatedAtAction(nameof(GetById), new { id = claim.Id }, claim);
        }

        [HttpPatch("{id:guid}/submit")]
        public async Task<IActionResult> Submit(Guid id)
        {
            Guid customerId = GetCurrentUserId();
            var claim = await _claimService.SubmitAsync(id, customerId);
            return Ok(claim);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateClaimStatusDto dto)
        {
            var claim = await _claimService.UpdateStatusAsync(id, dto);
            return Ok(claim);
        }

        private Guid GetCurrentUserId()
        {
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
                userIdStr = User.FindFirstValue("sub");
            if (userIdStr == null)
                throw new UnauthorizedAccessException("User identity not found.");
            return Guid.Parse(userIdStr);
        }
    }
}
