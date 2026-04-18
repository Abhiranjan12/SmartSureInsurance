using AdminService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(AdminDbContext context)
        {
            if (await context.Reports.AnyAsync())
                return;

            Guid adminId = Guid.Parse("44444444-4444-4444-4444-444444444401");

            var reports = new List<Report>
            {
                new Report { Title = "Monthly Claims Report - January 2025", ReportType = "Claims", Data = "{\"totalClaims\":45,\"approved\":30,\"rejected\":10,\"pending\":5,\"totalAmount\":2500000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-3) },
                new Report { Title = "Monthly Claims Report - February 2025", ReportType = "Claims", Data = "{\"totalClaims\":52,\"approved\":38,\"rejected\":8,\"pending\":6,\"totalAmount\":3100000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-2) },
                new Report { Title = "Monthly Claims Report - March 2025", ReportType = "Claims", Data = "{\"totalClaims\":61,\"approved\":44,\"rejected\":12,\"pending\":5,\"totalAmount\":3800000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-1) },
                new Report { Title = "Policy Sales Report - Q1 2025", ReportType = "Policy", Data = "{\"totalPolicies\":120,\"health\":40,\"vehicle\":35,\"life\":25,\"travel\":20,\"totalPremium\":4500000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-1) },
                new Report { Title = "Revenue Report - January 2025", ReportType = "Revenue", Data = "{\"totalPremiumCollected\":450000,\"totalClaimsPaid\":320000,\"netRevenue\":130000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-3) },
                new Report { Title = "Revenue Report - February 2025", ReportType = "Revenue", Data = "{\"totalPremiumCollected\":520000,\"totalClaimsPaid\":380000,\"netRevenue\":140000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-2) },
                new Report { Title = "Revenue Report - March 2025", ReportType = "Revenue", Data = "{\"totalPremiumCollected\":610000,\"totalClaimsPaid\":420000,\"netRevenue\":190000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-1) },
                new Report { Title = "User Registration Report - Q1 2025", ReportType = "Users", Data = "{\"totalUsers\":250,\"newUsers\":85,\"activeUsers\":210,\"customers\":240,\"admins\":10}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-1) },
                new Report { Title = "Claim Settlement Report - Q1 2025", ReportType = "Settlement", Data = "{\"avgSettlementDays\":12,\"fastestSettlement\":3,\"slowestSettlement\":45,\"settlementRate\":85}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-1) },
                new Report { Title = "Annual Summary Report - 2024", ReportType = "Annual", Data = "{\"totalPoliciesSold\":980,\"totalClaimsProcessed\":420,\"totalPremiumRevenue\":52000000,\"totalClaimsPaid\":38000000}", GeneratedBy = adminId, GeneratedAt = DateTime.UtcNow.AddMonths(-2) }
            };

            await context.Reports.AddRangeAsync(reports);
            await context.SaveChangesAsync();
        }
    }
}
