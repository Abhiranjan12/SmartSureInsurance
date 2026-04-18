using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyService.Application.DTOs;
using PolicyService.Application.Services;
using System.Security.Claims;

namespace PolicyService.API.Controllers
{
    [ApiController]
    [Route("api/policies")]
    [Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var policies = await _policyService.GetAllAsync();
            return Ok(policies);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyPolicies()
        {
            Guid customerId = GetCurrentUserId();
            var policies = await _policyService.GetByCustomerIdAsync(customerId);
            return Ok(policies);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var policy = await _policyService.GetByIdAsync(id);
            return Ok(policy);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] BuyPolicyDto dto)
        {
            Guid customerId = GetCurrentUserId();
            var policy = await _policyService.BuyPolicyAsync(customerId, dto);
            return CreatedAtAction(nameof(GetById), new { id = policy.Id }, policy);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdatePolicyStatusDto dto)
        {
            var updated = await _policyService.UpdateStatusAsync(id, dto);
            return Ok(updated);
        }

        private Guid GetCurrentUserId()
        {
            string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                userIdStr = User.FindFirstValue("sub");
            }
            if (userIdStr == null)
            {
                throw new UnauthorizedAccessException("User identity not found.");
            }
            return Guid.Parse(userIdStr);
        }
    }
}
