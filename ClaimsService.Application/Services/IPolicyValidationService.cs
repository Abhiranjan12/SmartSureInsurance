namespace ClaimsService.Application.Services
{
    public interface IPolicyValidationService
    {
        Task ValidatePolicyAsync(Guid policyId, Guid customerId);
    }
}
