using PolicyService.Application.DTOs;
using PolicyService.Application.Services;
using PolicyService.Domain.Repositories;

namespace PolicyService.Infrastructure.Services
{
    public class PremiumService : IPremiumService
    {
        private readonly IPolicyTypeRepository _policyTypeRepo;

        public PremiumService(IPolicyTypeRepository policyTypeRepo)
        {
            _policyTypeRepo = policyTypeRepo;
        }

        public async Task<PremiumResultDto> CalculateAsync(CalculatePremiumDto dto)
        {
            var policyType = await _policyTypeRepo.GetByIdAsync(dto.PolicyTypeId);
            if (policyType == null)
                throw new KeyNotFoundException("Policy type not found.");

            decimal calculatedPremium = CalculatePremium(
                policyType.BasePremium,
                dto.CustomerAge,
                dto.CoverageAmount,
                dto.DurationYears
            );

            return new PremiumResultDto
            {
                PolicyTypeId = policyType.Id,
                PolicyTypeName = policyType.Name,
                CoverageAmount = dto.CoverageAmount,
                DurationYears = dto.DurationYears,
                CalculatedPremium = calculatedPremium,
                MonthlyPremium = Math.Round(calculatedPremium / 12, 2)
            };
        }

        private decimal CalculatePremium(decimal basePremium, int age, decimal coverage, int years)
        {
            decimal ageFactor = age > 45 ? 1.3m : age > 30 ? 1.1m : 1.0m;
            decimal coverageFactor = coverage * 0.001m;
            decimal durationFactor = years * 0.05m;
            decimal annual = (basePremium + coverageFactor) * ageFactor * (1 + durationFactor);
            return Math.Round(annual, 2);
        }
    }
}
