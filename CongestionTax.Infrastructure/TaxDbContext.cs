using CongestionTax.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class TaxDbContext : DbContext
{
    public DbSet<TaxRule> TaxRules => Set<TaxRule>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<CityConfig> Cities => Set<CityConfig>();

    public TaxDbContext(DbContextOptions<TaxDbContext> opts) : base(opts) { }
}
