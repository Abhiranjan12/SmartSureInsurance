using AdminService.Application.DTOs;
using AdminService.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AdminService.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            string? token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var authClient = _httpClientFactory.CreateClient("AuthService");
            var policyClient = _httpClientFactory.CreateClient("PolicyService");
            var claimsClient = _httpClientFactory.CreateClient("ClaimsService");

            if (!string.IsNullOrEmpty(token))
            {
                string bearerToken = token.Replace("Bearer ", "");
                authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                policyClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                claimsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            var dashboard = new DashboardDto();

            var usersResponse = await authClient.GetAsync("/api/users");
            if (usersResponse.IsSuccessStatusCode)
            {
                var usersJson = await usersResponse.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<object>>(usersJson, options);
                dashboard.TotalUsers = users?.Count ?? 0;
            }

            var policiesResponse = await policyClient.GetAsync("/api/policies");
            if (policiesResponse.IsSuccessStatusCode)
            {
                var policiesJson = await policiesResponse.Content.ReadAsStringAsync();
                var policies = JsonSerializer.Deserialize<List<JsonElement>>(policiesJson, options);
                dashboard.TotalPolicies = policies?.Count ?? 0;

                if (policies != null)
                {
                    foreach (var p in policies)
                    {
                        if (p.TryGetProperty("premiumAmount", out JsonElement premium))
                            dashboard.TotalPremiumCollected += premium.GetDecimal();
                    }
                }
            }

            var claimsResponse = await claimsClient.GetAsync("/api/claims");
            if (claimsResponse.IsSuccessStatusCode)
            {
                var claimsJson = await claimsResponse.Content.ReadAsStringAsync();
                var claims = JsonSerializer.Deserialize<List<JsonElement>>(claimsJson, options);
                dashboard.TotalClaims = claims?.Count ?? 0;

                if (claims != null)
                {
                    foreach (var c in claims)
                    {
                        if (c.TryGetProperty("status", out JsonElement status))
                        {
                            string statusStr = status.GetString() ?? "";
                            if (statusStr == "Submitted" || statusStr == "UnderReview")
                                dashboard.PendingClaims++;
                            else if (statusStr == "Approved")
                                dashboard.ApprovedClaims++;
                            else if (statusStr == "Rejected")
                                dashboard.RejectedClaims++;
                        }

                        if (c.TryGetProperty("claimAmount", out JsonElement amount))
                            dashboard.TotalClaimAmount += amount.GetDecimal();
                    }
                }
            }

            return dashboard;
        }
    }
}
