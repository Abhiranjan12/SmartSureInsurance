using ClaimsService.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClaimsService.Infrastructure.Services
{
    public class PolicyValidationService : IPolicyValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PolicyValidationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ValidatePolicyAsync(Guid policyId, Guid customerId)
        {
            string? token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            var response = await _httpClient.GetAsync($"/api/policies/{policyId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException("Policy not found.");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to validate policy.");

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var policy = JsonSerializer.Deserialize<PolicyResponse>(content, options);

            if (policy == null)
                throw new InvalidOperationException("Unable to read policy data.");

            if (policy.CustomerId != customerId)
                throw new UnauthorizedAccessException("This policy does not belong to you.");

            if (policy.Status != "Active")
                throw new InvalidOperationException($"Claims can only be filed on Active policies. Current status: {policy.Status}");
        }

        private class PolicyResponse
        {
            public Guid Id { get; set; }
            public Guid CustomerId { get; set; }
            public string Status { get; set; } = string.Empty;
        }
    }
}
