using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wallet_tool.Data
{
    public class WalletAuthDbContext : IdentityDbContext
    {
        public WalletAuthDbContext(DbContextOptions<WalletAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var projectAdminId = "123e4566-e89b-12d3-a456-426614174001";
            var accountManagerId = "456e4566-e89b-12d3-a456-426614174566";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = accountManagerId,
                    ConcurrencyStamp = accountManagerId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = projectAdminId,
                    ConcurrencyStamp = projectAdminId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
