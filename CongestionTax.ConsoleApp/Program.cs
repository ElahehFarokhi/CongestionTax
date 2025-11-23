using CongestionTax.Application;
using CongestionTax.Domain;
using CongestionTax.Domain.Interfaces;
using CongestionTax.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();

services.AddDbContext<TaxDbContext>(options =>
    options.UseSqlite(config.GetConnectionString("TaxDb"))); // Use SQLite connection

services.AddScoped<ITaxCalculator, TaxCalculator>();

var provider = services.BuildServiceProvider();

using (var scope = provider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaxDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

// Load the rules for a specific city (Gothenburg in this case)
using var scope2 = provider.CreateScope();
var context = scope2.ServiceProvider.GetRequiredService<TaxDbContext>();

var city = context.Cities
    .Where(c => c.CityName == "Gothenburg")
    .Include(c => c.RuleSet)
    .ThenInclude(rs => rs.TaxRules)
    .Include(c => c.RuleSet)
    .ThenInclude(rs => rs.Holidays)
    .FirstOrDefault();

// Get the TaxCalculator service
var calc = provider.GetRequiredService<ITaxCalculator>();

// Example: Load timestamps from a file called ExampleDates.txt
List<DateTime> timestamps = File.ReadAllLines("ExampleDates.txt")
    .Select(DateTime.Parse)
    .ToList();

// Create a vehicle (for example, a car)
var vehicle = new Vehicle { Type = VehicleType.Car };

// Calculate the congestion tax
int fee = calc.CalculateTax(vehicle, timestamps, city.RuleSet);

// Output the result to the console
Console.WriteLine($"Congestion tax for today: {fee}");
Console.ReadLine();  // Keep the console open
