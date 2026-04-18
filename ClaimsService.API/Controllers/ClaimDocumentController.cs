using ClaimsService.Application.DTOs;
using ClaimsService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsService.API.Controllers
{
    [ApiController]
    [Route("api/claims/{claimId:guid}/documents")]
    [Authorize]
    public class ClaimDocumentController : ControllerBase
    {
        private readonly IClaimDocumentService _documentService;

        public ClaimDocumentController(IClaimDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByClaimId(Guid claimId)
        {
            var docs = await _documentService.GetByClaimIdAsync(claimId);
            return Ok(docs);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(Guid claimId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var doc = await _documentService.UploadAsync(claimId, file);
            return Ok(doc);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{documentId:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid claimId, Guid documentId, [FromBody] UpdateDocumentStatusDto dto)
        {
            var doc = await _documentService.UpdateStatusAsync(documentId, dto);
            return Ok(doc);
        }

        [HttpDelete("{documentId:guid}")]
        public async Task<IActionResult> Delete(Guid claimId, Guid documentId)
        {
            await _documentService.DeleteAsync(documentId);
            return NoContent();
        }
    }
}
