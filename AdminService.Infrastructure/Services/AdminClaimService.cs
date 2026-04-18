using AdminService.Application.DTOs;
using AdminService.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AdminService.Infrastructure.Services
{
    public class AdminClaimService : IAdminClaimService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminClaimService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ClaimsService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ClaimSummaryDto>> GetAllClaimsAsync()
        {
            AttachToken();
            var response = await _httpClient.GetAsync("/api/claims");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<List<ClaimSummaryDto>>(content, options);
            return result ?? new List<ClaimSummaryDto>();
        }

        public async Task<ClaimSummaryDto> GetClaimByIdAsync(Guid claimId)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"/api/claims/{claimId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException("Claim not found.");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ClaimSummaryDto>(content, options);
            return result ?? throw new InvalidOperationException("Unable to read claim data.");
        }

        public async Task<ClaimSummaryDto> UpdateClaimStatusAsync(Guid claimId, UpdateClaimStatusDto dto)
        {
            AttachToken();
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"/api/claims/{claimId}/status", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException("Claim not found.");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ClaimSummaryDto>(responseContent, options);
            return result ?? throw new InvalidOperationException("Unable to read updated claim.");
        }

        private void AttachToken()
        {
            string? token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
        }
    }
}
