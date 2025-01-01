using System.Data;
using Microsoft.EntityFrameworkCore;
using Wallet_tool.Model.Domain;

namespace Wallet_tool.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {

        }
        public DbSet<WalletAccount> WalletAccount { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WalletAccount>()
                .Property(w => w.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WalletAccount>()
                .Property(w => w.Expenditure)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);
        }
    }
}
