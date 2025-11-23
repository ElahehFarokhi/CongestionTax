using CongestionTax.Domain;
using Microsoft.EntityFrameworkCore;

public class TaxDbContext : DbContext
{
    public DbSet<TaxRule> TaxRules => Set<TaxRule>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<CityConfig> Cities => Set<CityConfig>();

    public TaxDbContext(DbContextOptions<TaxDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuring CityConfig to RuleSet relationship (One-to-One)
        modelBuilder.Entity<CityConfig>()
            .HasOne(c => c.RuleSet)
            .WithOne()
            .HasForeignKey<CityConfig>(c => c.RuleSetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuring RuleSet to TaxRules (One-to-Many)
        modelBuilder.Entity<RuleSet>()
            .HasMany(r => r.TaxRules)
            .WithOne()
            .HasForeignKey(t => t.RuleSetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuring RuleSet to Holidays (One-to-Many)
        modelBuilder.Entity<RuleSet>()
            .HasMany(r => r.Holidays)
            .WithOne()
            .HasForeignKey(h => h.RuleSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
