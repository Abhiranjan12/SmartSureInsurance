using PolicyService.Application.DTOs;
using PolicyService.Application.Services;
using PolicyService.Domain.Entities;
using PolicyService.Domain.Repositories;

namespace PolicyService.Infrastructure.Services
{
    public class PolicyTypeService : IPolicyTypeService
    {
        private readonly IPolicyTypeRepository _repo;

        public PolicyTypeService(IPolicyTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PolicyTypeDto>> GetAllAsync()
        {
            var types = await _repo.GetAllAsync();
            var result = new List<PolicyTypeDto>();

            foreach (var t in types)
            {
                result.Add(MapToDto(t));
            }

            return result;
        }

        public async Task<PolicyTypeDto> GetByIdAsync(Guid id)
        {
            var policyType = await _repo.GetByIdAsync(id);
            if (policyType == null)
                throw new KeyNotFoundException("Policy type not found.");

            return MapToDto(policyType);
        }

        public async Task<PolicyTypeDto> CreateAsync(CreatePolicyTypeDto dto)
        {
            var policyType = new PolicyType
            {
                Name = dto.Name,
                Description = dto.Description,
                BasePremium = dto.BasePremium,
                CoverageYears = dto.CoverageYears,
                IsActive = true
            };

            await _repo.AddAsync(policyType);
            return MapToDto(policyType);
        }

        public async Task<PolicyTypeDto> UpdateAsync(Guid id, UpdatePolicyTypeDto dto)
        {
            var policyType = await _repo.GetByIdAsync(id);
            if (policyType == null)
                throw new KeyNotFoundException("Policy type not found.");

            policyType.Name = dto.Name;
            policyType.Description = dto.Description;
            policyType.BasePremium = dto.BasePremium;
            policyType.CoverageYears = dto.CoverageYears;
            policyType.IsActive = dto.IsActive;

            await _repo.UpdateAsync(policyType);
            return MapToDto(policyType);
        }

        public async Task DeleteAsync(Guid id)
        {
            var policyType = await _repo.GetByIdAsync(id);
            if (policyType == null)
                throw new KeyNotFoundException("Policy type not found.");

            await _repo.DeleteAsync(policyType);
        }

        private PolicyTypeDto MapToDto(PolicyType t)
        {
            return new PolicyTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                BasePremium = t.BasePremium,
                CoverageYears = t.CoverageYears,
                IsActive = t.IsActive
            };
        }
    }
}
