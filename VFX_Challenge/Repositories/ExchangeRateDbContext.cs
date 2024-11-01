using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VFX_Challenge.Models;

namespace VFX_Challenge.Repositories
{
    public class ExchangeRateDbContext : DbContext
    {
        public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options) : base(options) { }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}

