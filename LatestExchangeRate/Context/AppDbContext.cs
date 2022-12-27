using LatestExchangeRate.Entities;
using LatestExchangeRate.Models;
using Microsoft.EntityFrameworkCore;


namespace LatestExchangeRate.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<FixerResponseEntity> FixerResponses { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>()
                .HasOne(cr => cr.FixerResponseEntity)
                .WithMany(fr => fr.CurrencyRates)
                .HasForeignKey(cr => cr.FixerResponseEntityId);
        }
    }
}
