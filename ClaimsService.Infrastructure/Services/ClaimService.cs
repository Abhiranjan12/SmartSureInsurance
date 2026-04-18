using ClaimsService.Application.DTOs;
using ClaimsService.Application.Services;
using ClaimsService.Domain.Entities;
using ClaimsService.Domain.Repositories;
using MassTransit;
using SmartSure.Contracts.Events;

namespace ClaimsService.Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IPolicyValidationService _policyValidation;
        private readonly IPublishEndpoint _publishEndpoint;

        public ClaimService(IClaimRepository claimRepo, IPolicyValidationService policyValidation, IPublishEndpoint publishEndpoint)
        {
            _claimRepo = claimRepo;
            _policyValidation = policyValidation;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<ClaimDto>> GetAllAsync()
        {
            var claims = await _claimRepo.GetAllAsync();
            var result = new List<ClaimDto>();
            foreach (var c in claims)
                result.Add(MapToDto(c));
            return result;
        }

        public async Task<IEnumerable<ClaimDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var claims = await _claimRepo.GetByCustomerIdAsync(customerId);
            var result = new List<ClaimDto>();
            foreach (var c in claims)
                result.Add(MapToDto(c));
            return result;
        }

        public async Task<ClaimDto> GetByIdAsync(Guid id)
        {
            var claim = await _claimRepo.GetByIdAsync(id);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");
            return MapToDto(claim);
        }

        public async Task<ClaimDto> CreateAsync(Guid customerId, CreateClaimDto dto)
        {
            await _policyValidation.ValidatePolicyAsync(dto.PolicyId, customerId);

            var claim = new Claim
            {
                CustomerId = customerId,
                PolicyId = dto.PolicyId,
                ClaimNumber = GenerateClaimNumber(),
                IncidentDescription = dto.IncidentDescription,
                IncidentDate = dto.IncidentDate,
                ClaimAmount = dto.ClaimAmount,
                Status = ClaimStatus.Draft
            };

            await _claimRepo.AddAsync(claim);
            return MapToDto(claim);
        }

        public async Task<ClaimDto> SubmitAsync(Guid claimId, Guid customerId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            if (claim.CustomerId != customerId)
                throw new UnauthorizedAccessException("You can only submit your own claims.");

            if (claim.Status != ClaimStatus.Draft)
                throw new InvalidOperationException("Only Draft claims can be submitted.");

            claim.Status = ClaimStatus.Submitted;
            await _claimRepo.UpdateAsync(claim);

            await _publishEndpoint.Publish(new ClaimSubmittedEvent
            {
                ClaimId = claim.Id,
                CustomerId = claim.CustomerId,
                PolicyId = claim.PolicyId,
                ClaimNumber = claim.ClaimNumber,
                ClaimAmount = claim.ClaimAmount,
                IncidentDescription = claim.IncidentDescription
            });

            return MapToDto(claim);
        }

        public async Task<ClaimDto> UpdateStatusAsync(Guid claimId, UpdateClaimStatusDto dto)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            bool parsed = Enum.TryParse<ClaimStatus>(dto.Status, true, out ClaimStatus newStatus);
            if (parsed == false)
                throw new ArgumentException("Invalid claim status.");

            ValidateStatusTransition(claim.Status, newStatus);

            claim.Status = newStatus;
            claim.AdminRemarks = dto.AdminRemarks;

            if (newStatus == ClaimStatus.Rejected)
                claim.RejectionReason = dto.RejectionReason;

            await _claimRepo.UpdateAsync(claim);

            await _publishEndpoint.Publish(new ClaimStatusUpdatedEvent
            {
                ClaimId = claim.Id,
                CustomerId = claim.CustomerId,
                ClaimNumber = claim.ClaimNumber,
                NewStatus = newStatus.ToString(),
                RejectionReason = claim.RejectionReason,
                CustomerEmail = string.Empty,
                CustomerName = string.Empty
            });

            if (newStatus == ClaimStatus.Approved)
            {
                await _publishEndpoint.Publish(new ClaimApprovedEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    ClaimId = claim.Id,
                    CustomerId = claim.CustomerId,
                    PolicyId = claim.PolicyId,
                    ClaimNumber = claim.ClaimNumber,
                    ApprovedAmount = claim.ClaimAmount,
                    CustomerEmail = string.Empty,
                    CustomerName = string.Empty
                });
            }
            else if (newStatus == ClaimStatus.Rejected)
            {
                await _publishEndpoint.Publish(new ClaimRejectedEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    ClaimId = claim.Id,
                    CustomerId = claim.CustomerId,
                    ClaimNumber = claim.ClaimNumber,
                    RejectionReason = claim.RejectionReason ?? string.Empty,
                    CustomerEmail = string.Empty,
                    CustomerName = string.Empty
                });
            }

            return MapToDto(claim);
        }

        private void ValidateStatusTransition(ClaimStatus current, ClaimStatus next)
        {
            bool valid = false;

            if (current == ClaimStatus.Submitted && next == ClaimStatus.UnderReview)
                valid = true;
            else if (current == ClaimStatus.UnderReview && next == ClaimStatus.Approved)
                valid = true;
            else if (current == ClaimStatus.UnderReview && next == ClaimStatus.Rejected)
                valid = true;
            else if (current == ClaimStatus.Approved && next == ClaimStatus.Closed)
                valid = true;
            else if (current == ClaimStatus.Rejected && next == ClaimStatus.Closed)
                valid = true;

            if (valid == false)
                throw new InvalidOperationException($"Cannot transition from {current} to {next}.");
        }

        private string GenerateClaimNumber()
        {
            return "CLM-" + DateTime.UtcNow.Year + "-" + Random.Shared.Next(100000, 999999).ToString();
        }

        private ClaimDto MapToDto(Claim c)
        {
            var docs = new List<ClaimDocumentDto>();
            foreach (var d in c.Documents)
            {
                docs.Add(new ClaimDocumentDto
                {
                    Id = d.Id,
                    ClaimId = d.ClaimId,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    FileSizeBytes = d.FileSizeBytes,
                    Status = d.Status.ToString(),
                    UploadedAt = d.UploadedAt
                });
            }

            return new ClaimDto
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                PolicyId = c.PolicyId,
                ClaimNumber = c.ClaimNumber,
                IncidentDescription = c.IncidentDescription,
                IncidentDate = c.IncidentDate,
                ClaimAmount = c.ClaimAmount,
                Status = c.Status.ToString(),
                RejectionReason = c.RejectionReason,
                AdminRemarks = c.AdminRemarks,
                CreatedAt = c.CreatedAt,
                Documents = docs
            };
        }
    }
}
