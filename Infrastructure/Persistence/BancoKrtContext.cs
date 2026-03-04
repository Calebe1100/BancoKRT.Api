using BancoKRT.Api.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace BancoKRT.Api.Infrastructure.Persistence
{
    public class BancoKrtContext : DbContext
    {
        public BancoKrtContext(DbContextOptions<BancoKrtContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(builder =>
            {
                builder.ToTable("Accounts");
                builder.HasKey(a => a.Id);
                builder.Property(a => a.HolderName).IsRequired().HasMaxLength(200);
                builder.Property(a => a.Cpf).IsRequired().HasMaxLength(11);
                builder.Property(a => a.Status).IsRequired();
            });
        }
    }
}
