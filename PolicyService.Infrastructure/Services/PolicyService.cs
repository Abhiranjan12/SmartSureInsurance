using MassTransit;
using PolicyService.Application.DTOs;
using PolicyService.Application.Services;
using PolicyService.Domain.Entities;
using PolicyService.Domain.Repositories;

namespace PolicyService.Infrastructure.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IPolicyTypeRepository _policyTypeRepo;
        private readonly IPremiumService _premiumService;
        private readonly IPublishEndpoint _publishEndpoint;

        public PolicyService(
            IPolicyRepository policyRepo,
            IPolicyTypeRepository policyTypeRepo,
            IPremiumService premiumService,
            IPublishEndpoint publishEndpoint)
        {
            _policyRepo = policyRepo;
            _policyTypeRepo = policyTypeRepo;
            _premiumService = premiumService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<PolicyDto>> GetAllAsync()
        {
            var policies = await _policyRepo.GetAllAsync();
            var result = new List<PolicyDto>();
            foreach (var p in policies)
                result.Add(MapToDto(p));
            return result;
        }

        public async Task<IEnumerable<PolicyDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var policies = await _policyRepo.GetByCustomerIdAsync(customerId);
            var result = new List<PolicyDto>();
            foreach (var p in policies)
                result.Add(MapToDto(p));
            return result;
        }

        public async Task<PolicyDto> GetByIdAsync(Guid id)
        {
            var policy = await _policyRepo.GetByIdAsync(id);
            if (policy == null)
                throw new KeyNotFoundException("Policy not found.");
            return MapToDto(policy);
        }

        public async Task<PolicyDto> BuyPolicyAsync(Guid customerId, BuyPolicyDto dto)
        {
            var policyType = await _policyTypeRepo.GetByIdAsync(dto.PolicyTypeId);
            if (policyType == null)
            {
                await _publishEndpoint.Publish(new SmartSure.Contracts.Events.PolicyPurchaseFailedEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    CustomerId = customerId,
                    Reason = "Policy type not found."
                });
                throw new KeyNotFoundException("Policy type not found.");
            }

            if (policyType.IsActive == false)
            {
                await _publishEndpoint.Publish(new SmartSure.Contracts.Events.PolicyPurchaseFailedEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    CustomerId = customerId,
                    Reason = "This policy type is not available."
                });
                throw new InvalidOperationException("This policy type is not available.");
            }

            var premiumRequest = new CalculatePremiumDto
            {
                PolicyTypeId = dto.PolicyTypeId,
                CustomerAge = dto.CustomerAge,
                CoverageAmount = dto.CoverageAmount,
                DurationYears = dto.DurationYears
            };

            var premiumResult = await _premiumService.CalculateAsync(premiumRequest);

            var policy = new Policy
            {
                CustomerId = customerId,
                PolicyTypeId = dto.PolicyTypeId,
                PolicyNumber = GeneratePolicyNumber(),
                Status = PolicyStatus.Active,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(dto.DurationYears),
                PremiumAmount = premiumResult.CalculatedPremium,
                CoverageAmount = dto.CoverageAmount
            };

            await _policyRepo.AddAsync(policy);
            policy.PolicyType = policyType;

            await _publishEndpoint.Publish(new SmartSure.Contracts.Events.PolicyPurchasedEvent
            {
                PolicyId = policy.Id,
                CustomerId = policy.CustomerId,
                PolicyNumber = policy.PolicyNumber,
                PolicyTypeName = policyType.Name,
                PremiumAmount = policy.PremiumAmount,
                CoverageAmount = policy.CoverageAmount,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate
            });

            return MapToDto(policy);
        }

        public async Task<PolicyDto> UpdateStatusAsync(Guid id, UpdatePolicyStatusDto dto)
        {
            var policy = await _policyRepo.GetByIdAsync(id);
            if (policy == null)
                throw new KeyNotFoundException("Policy not found.");

            bool parsed = Enum.TryParse<PolicyStatus>(dto.Status, true, out PolicyStatus newStatus);
            if (parsed == false)
                throw new ArgumentException("Invalid policy status.");

            policy.Status = newStatus;
            await _policyRepo.UpdateAsync(policy);
            return MapToDto(policy);
        }

        private string GeneratePolicyNumber()
        {
            return "POL-" + DateTime.UtcNow.Year + "-" + Random.Shared.Next(100000, 999999).ToString();
        }

        private PolicyDto MapToDto(Policy p)
        {
            return new PolicyDto
            {
                Id = p.Id,
                CustomerId = p.CustomerId,
                PolicyNumber = p.PolicyNumber,
                PolicyTypeName = p.PolicyType != null ? p.PolicyType.Name : string.Empty,
                Status = p.Status.ToString(),
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                PremiumAmount = p.PremiumAmount,
                CoverageAmount = p.CoverageAmount,
                CreatedAt = p.CreatedAt
            };
        }
    }
}
