using Microsoft.EntityFrameworkCore;
using PolicyService.Domain.Entities;

namespace PolicyService.Infrastructure.Data
{
    public static class PolicySeeder
    {
        public static async Task SeedAsync(PolicyDbContext context)
        {
            await SeedPolicyTypesAsync(context);
            await SeedPoliciesAsync(context);
            await SeedPremiumsAsync(context);
        }

        private static async Task SeedPolicyTypesAsync(PolicyDbContext context)
        {
            if (await context.PolicyTypes.AnyAsync())
                return;

            var types = new List<PolicyType>
            {
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111101"), Name = "Health Insurance", Description = "Covers medical expenses and hospitalization costs", BasePremium = 5000, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111102"), Name = "Vehicle Insurance", Description = "Covers damage, theft and accidents for vehicles", BasePremium = 3000, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111103"), Name = "Life Insurance", Description = "Provides financial security to family on death", BasePremium = 8000, CoverageYears = 10, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111104"), Name = "Travel Insurance", Description = "Covers travel risks, delays and emergencies", BasePremium = 1500, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111105"), Name = "Home Insurance", Description = "Covers home and property from damage and theft", BasePremium = 4000, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111106"), Name = "Term Insurance", Description = "Pure life cover for a fixed term period", BasePremium = 6000, CoverageYears = 20, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111107"), Name = "Critical Illness", Description = "Covers diagnosis of critical illness like cancer", BasePremium = 7000, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111108"), Name = "Personal Accident", Description = "Covers accidental injuries and permanent disability", BasePremium = 2000, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111109"), Name = "Fire Insurance", Description = "Covers fire damage to property and assets", BasePremium = 3500, CoverageYears = 1, IsActive = true },
                new PolicyType { Id = Guid.Parse("11111111-1111-1111-1111-111111111110"), Name = "Marine Insurance", Description = "Covers goods and cargo in transit by sea", BasePremium = 4500, CoverageYears = 1, IsActive = true }
            };

            await context.PolicyTypes.AddRangeAsync(types);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPoliciesAsync(PolicyDbContext context)
        {
            if (await context.Policies.AnyAsync())
                return;

            var customerIds = new List<Guid>
            {
                Guid.Parse("22222222-2222-2222-2222-222222222201"),
                Guid.Parse("22222222-2222-2222-2222-222222222202"),
                Guid.Parse("22222222-2222-2222-2222-222222222203"),
                Guid.Parse("22222222-2222-2222-2222-222222222204"),
                Guid.Parse("22222222-2222-2222-2222-222222222205")
            };

            var policies = new List<Policy>
            {
                new Policy { CustomerId = customerIds[0], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111101"), PolicyNumber = "POL-2025-100001", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-6), EndDate = DateTime.UtcNow.AddMonths(6), PremiumAmount = 5500, CoverageAmount = 500000 },
                new Policy { CustomerId = customerIds[1], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111102"), PolicyNumber = "POL-2025-100002", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-3), EndDate = DateTime.UtcNow.AddMonths(9), PremiumAmount = 3300, CoverageAmount = 200000 },
                new Policy { CustomerId = customerIds[2], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111103"), PolicyNumber = "POL-2025-100003", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddYears(9), PremiumAmount = 9600, CoverageAmount = 1000000 },
                new Policy { CustomerId = customerIds[3], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111104"), PolicyNumber = "POL-2025-100004", Status = PolicyStatus.Expired, StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddDays(-30), PremiumAmount = 1650, CoverageAmount = 100000 },
                new Policy { CustomerId = customerIds[4], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111105"), PolicyNumber = "POL-2025-100005", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-2), EndDate = DateTime.UtcNow.AddMonths(10), PremiumAmount = 4400, CoverageAmount = 300000 },
                new Policy { CustomerId = customerIds[0], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111107"), PolicyNumber = "POL-2025-100006", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-1), EndDate = DateTime.UtcNow.AddMonths(11), PremiumAmount = 7700, CoverageAmount = 750000 },
                new Policy { CustomerId = customerIds[1], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111108"), PolicyNumber = "POL-2025-100007", Status = PolicyStatus.Cancelled, StartDate = DateTime.UtcNow.AddMonths(-8), EndDate = DateTime.UtcNow.AddMonths(4), PremiumAmount = 2200, CoverageAmount = 150000 },
                new Policy { CustomerId = customerIds[2], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111109"), PolicyNumber = "POL-2025-100008", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-4), EndDate = DateTime.UtcNow.AddMonths(8), PremiumAmount = 3850, CoverageAmount = 400000 },
                new Policy { CustomerId = customerIds[3], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111106"), PolicyNumber = "POL-2025-100009", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddYears(-2), EndDate = DateTime.UtcNow.AddYears(18), PremiumAmount = 7200, CoverageAmount = 2000000 },
                new Policy { CustomerId = customerIds[4], PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111110"), PolicyNumber = "POL-2025-100010", Status = PolicyStatus.Active, StartDate = DateTime.UtcNow.AddMonths(-5), EndDate = DateTime.UtcNow.AddMonths(7), PremiumAmount = 4950, CoverageAmount = 600000 }
            };

            await context.Policies.AddRangeAsync(policies);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPremiumsAsync(PolicyDbContext context)
        {
            if (await context.Premiums.AnyAsync())
                return;

            var premiums = new List<Premium>
            {
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111101"), CustomerAge = 28, CoverageAmount = 500000, CalculatedPremium = 5500, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111102"), CustomerAge = 32, CoverageAmount = 200000, CalculatedPremium = 3300, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111103"), CustomerAge = 45, CoverageAmount = 1000000, CalculatedPremium = 9600, DurationYears = 10 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111104"), CustomerAge = 26, CoverageAmount = 100000, CalculatedPremium = 1650, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111105"), CustomerAge = 38, CoverageAmount = 300000, CalculatedPremium = 4400, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111107"), CustomerAge = 28, CoverageAmount = 750000, CalculatedPremium = 7700, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111108"), CustomerAge = 32, CoverageAmount = 150000, CalculatedPremium = 2200, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111109"), CustomerAge = 45, CoverageAmount = 400000, CalculatedPremium = 3850, DurationYears = 1 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111106"), CustomerAge = 34, CoverageAmount = 2000000, CalculatedPremium = 7200, DurationYears = 20 },
                new Premium { PolicyTypeId = Guid.Parse("11111111-1111-1111-1111-111111111110"), CustomerAge = 41, CoverageAmount = 600000, CalculatedPremium = 4950, DurationYears = 1 }
            };

            await context.Premiums.AddRangeAsync(premiums);
            await context.SaveChangesAsync();
        }
    }
}
