using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Persistence.Context
{
    public class SaviDbContext : IdentityDbContext<AppUser>
    {
        public SaviDbContext(DbContextOptions<SaviDbContext> options) : base(options) { }

        public DbSet<CardDetail> CardDetails { get; set; }
        public DbSet<GroupTransaction> GroupTransactions {get; set;}
        public DbSet<KYC> KYCs { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletFunding> WalletFundings { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }
        public DbSet<GroupSavings> GroupSavings { get; set; }

        public DbSet<GroupSavingsFunding> GroupSavingsFunding { get; set; }

        public DbSet<GroupSavingsMembers> GroupSavingsMembers { get; set; }






    }



}
