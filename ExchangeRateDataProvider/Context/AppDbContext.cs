using ExchangeRateDataProvider.Entities;
using ExchangeRateDataProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateDataProvider.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FixerResponseEntity> FixerResponses { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}
