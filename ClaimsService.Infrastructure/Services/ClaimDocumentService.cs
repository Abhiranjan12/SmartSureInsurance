using ClaimsService.Application.DTOs;
using ClaimsService.Application.Services;
using ClaimsService.Domain.Entities;
using ClaimsService.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace ClaimsService.Infrastructure.Services
{
    public class ClaimDocumentService : IClaimDocumentService
    {
        private readonly IClaimDocumentRepository _docRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly string _uploadPath;

        public ClaimDocumentService(IClaimDocumentRepository docRepo, IClaimRepository claimRepo)
        {
            _docRepo = docRepo;
            _claimRepo = claimRepo;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Claims");
        }

        public async Task<IEnumerable<ClaimDocumentDto>> GetByClaimIdAsync(Guid claimId)
        {
            var docs = await _docRepo.GetByClaimIdAsync(claimId);
            var result = new List<ClaimDocumentDto>();
            foreach (var d in docs)
                result.Add(MapToDto(d));
            return result;
        }

        public async Task<ClaimDocumentDto> UploadAsync(Guid claimId, IFormFile file)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            if (claim.Status != ClaimStatus.Draft && claim.Status != ClaimStatus.Submitted)
                throw new InvalidOperationException("Documents can only be uploaded for Draft or Submitted claims.");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_uploadPath, uniqueFileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new ClaimDocument
            {
                ClaimId = claimId,
                FileName = file.FileName,
                FilePath = filePath,
                FileType = file.ContentType,
                FileSizeBytes = file.Length,
                Status = DocumentStatus.Pending
            };

            await _docRepo.AddAsync(document);
            return MapToDto(document);
        }

        public async Task<ClaimDocumentDto> UpdateStatusAsync(Guid documentId, UpdateDocumentStatusDto dto)
        {
            var document = await _docRepo.GetByIdAsync(documentId);
            if (document == null)
                throw new KeyNotFoundException("Document not found.");

            bool parsed = Enum.TryParse<DocumentStatus>(dto.Status, true, out DocumentStatus newStatus);
            if (parsed == false)
                throw new ArgumentException("Invalid document status.");

            document.Status = newStatus;
            await _docRepo.UpdateAsync(document);
            return MapToDto(document);
        }

        public async Task DeleteAsync(Guid documentId)
        {
            var document = await _docRepo.GetByIdAsync(documentId);
            if (document == null)
                throw new KeyNotFoundException("Document not found.");

            if (File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            await _docRepo.DeleteAsync(document);
        }

        private ClaimDocumentDto MapToDto(ClaimDocument d)
        {
            return new ClaimDocumentDto
            {
                Id = d.Id,
                ClaimId = d.ClaimId,
                FileName = d.FileName,
                FileType = d.FileType,
                FileSizeBytes = d.FileSizeBytes,
                Status = d.Status.ToString(),
                UploadedAt = d.UploadedAt
            };
        }
    }
}
