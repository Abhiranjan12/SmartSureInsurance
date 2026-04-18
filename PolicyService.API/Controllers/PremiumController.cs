using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyService.Application.DTOs;
using PolicyService.Application.Services;

namespace PolicyService.API.Controllers
{
    [ApiController]
    [Route("api/premiums")]
    [Authorize]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumService _premiumService;

        public PremiumController(IPremiumService premiumService)
        {
            _premiumService = premiumService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] CalculatePremiumDto dto)
        {
            var result = await _premiumService.CalculateAsync(dto);
            return Ok(result);
        }
    }
}
