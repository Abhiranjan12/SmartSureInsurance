using ClaimsService.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace ClaimsService.Application.Services
{
    public interface IClaimDocumentService
    {
        Task<IEnumerable<ClaimDocumentDto>> GetByClaimIdAsync(Guid claimId);
        Task<ClaimDocumentDto> UploadAsync(Guid claimId, IFormFile file);
        Task<ClaimDocumentDto> UpdateStatusAsync(Guid documentId, UpdateDocumentStatusDto dto);
        Task DeleteAsync(Guid documentId);
    }
}
