using Microsoft.EntityFrameworkCore;
using PolicyService.Domain.Entities;

namespace PolicyService.Infrastructure.Data
{
    public class PolicyDbContext : DbContext
    {
        public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options) { }

        public DbSet<PolicyType> PolicyTypes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Premium> Premiums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.PolicyType)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Premium>()
                .HasOne(p => p.PolicyType)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasIndex(p => p.PolicyNumber)
                .IsUnique();
        }
    }
}
