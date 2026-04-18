using ClaimsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaimsService.Infrastructure.Data
{
    public static class ClaimsSeeder
    {
        public static async Task SeedAsync(ClaimsDbContext context)
        {
            await SeedClaimsAsync(context);
        }

        private static async Task SeedClaimsAsync(ClaimsDbContext context)
        {
            if (await context.Claims.AnyAsync())
                return;

            var customerIds = new List<Guid>
            {
                Guid.Parse("22222222-2222-2222-2222-222222222201"),
                Guid.Parse("22222222-2222-2222-2222-222222222202"),
                Guid.Parse("22222222-2222-2222-2222-222222222203"),
                Guid.Parse("22222222-2222-2222-2222-222222222204"),
                Guid.Parse("22222222-2222-2222-2222-222222222205")
            };

            var policyIds = new List<Guid>
            {
                Guid.Parse("33333333-3333-3333-3333-333333333301"),
                Guid.Parse("33333333-3333-3333-3333-333333333302"),
                Guid.Parse("33333333-3333-3333-3333-333333333303"),
                Guid.Parse("33333333-3333-3333-3333-333333333304"),
                Guid.Parse("33333333-3333-3333-3333-333333333305")
            };

            var claims = new List<Claim>
            {
                new Claim { CustomerId = customerIds[0], PolicyId = policyIds[0], ClaimNumber = "CLM-2025-100001", IncidentDescription = "Vehicle accident on highway near toll booth", IncidentDate = DateTime.UtcNow.AddDays(-30), ClaimAmount = 50000, Status = ClaimStatus.Approved, AdminRemarks = "Valid claim, documents verified" },
                new Claim { CustomerId = customerIds[1], PolicyId = policyIds[1], ClaimNumber = "CLM-2025-100002", IncidentDescription = "Hospitalization due to dengue fever for 5 days", IncidentDate = DateTime.UtcNow.AddDays(-25), ClaimAmount = 30000, Status = ClaimStatus.UnderReview },
                new Claim { CustomerId = customerIds[2], PolicyId = policyIds[2], ClaimNumber = "CLM-2025-100003", IncidentDescription = "Home fire damage in kitchen area", IncidentDate = DateTime.UtcNow.AddDays(-20), ClaimAmount = 150000, Status = ClaimStatus.Submitted },
                new Claim { CustomerId = customerIds[3], PolicyId = policyIds[3], ClaimNumber = "CLM-2025-100004", IncidentDescription = "Travel luggage lost at airport", IncidentDate = DateTime.UtcNow.AddDays(-15), ClaimAmount = 20000, Status = ClaimStatus.Rejected, RejectionReason = "Insufficient supporting documents provided" },
                new Claim { CustomerId = customerIds[4], PolicyId = policyIds[4], ClaimNumber = "CLM-2025-100005", IncidentDescription = "Vehicle theft from parking lot", IncidentDate = DateTime.UtcNow.AddDays(-10), ClaimAmount = 200000, Status = ClaimStatus.Draft },
                new Claim { CustomerId = customerIds[0], PolicyId = policyIds[0], ClaimNumber = "CLM-2025-100006", IncidentDescription = "Critical illness diagnosis - Type 2 Diabetes", IncidentDate = DateTime.UtcNow.AddDays(-8), ClaimAmount = 100000, Status = ClaimStatus.Submitted },
                new Claim { CustomerId = customerIds[1], PolicyId = policyIds[1], ClaimNumber = "CLM-2025-100007", IncidentDescription = "Accidental injury at workplace - broken arm", IncidentDate = DateTime.UtcNow.AddDays(-6), ClaimAmount = 45000, Status = ClaimStatus.UnderReview },
                new Claim { CustomerId = customerIds[2], PolicyId = policyIds[2], ClaimNumber = "CLM-2025-100008", IncidentDescription = "Flood damage to property and furniture", IncidentDate = DateTime.UtcNow.AddDays(-4), ClaimAmount = 80000, Status = ClaimStatus.Approved, AdminRemarks = "Claim approved after site inspection" },
                new Claim { CustomerId = customerIds[3], PolicyId = policyIds[3], ClaimNumber = "CLM-2025-100009", IncidentDescription = "Marine cargo damage during sea transit", IncidentDate = DateTime.UtcNow.AddDays(-2), ClaimAmount = 60000, Status = ClaimStatus.Closed },
                new Claim { CustomerId = customerIds[4], PolicyId = policyIds[4], ClaimNumber = "CLM-2025-100010", IncidentDescription = "Fire at warehouse causing stock damage", IncidentDate = DateTime.UtcNow.AddDays(-1), ClaimAmount = 250000, Status = ClaimStatus.Submitted }
            };

            await context.Claims.AddRangeAsync(claims);
            await context.SaveChangesAsync();
        }
    }
}
