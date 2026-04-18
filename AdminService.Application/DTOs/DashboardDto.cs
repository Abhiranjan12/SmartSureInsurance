namespace AdminService.Application.DTOs
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalPolicies { get; set; }
        public int TotalClaims { get; set; }
        public int PendingClaims { get; set; }
        public int ApprovedClaims { get; set; }
        public int RejectedClaims { get; set; }
        public decimal TotalPremiumCollected { get; set; }
        public decimal TotalClaimAmount { get; set; }
    }
}
