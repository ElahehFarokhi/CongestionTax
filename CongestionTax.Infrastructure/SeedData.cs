using CongestionTax.Domain;

namespace CongestionTax.Infrastructure;

public static class SeedData
{
    public static void Initialize(TaxDbContext db)
    {

        if (!db.Cities.Any())
        {
            // Define tax rules for Gothenburg
            var gothenburgRuleSet = new RuleSet
            {
                MaxDailyFee = 60,
                SingleChargeMinutes = 60,
                TaxRules =
                [
                    new TaxRule { Start = TimeSpan.Parse("06:00"), End = TimeSpan.Parse("06:29"), Amount = 8 },
                    new TaxRule { Start = TimeSpan.Parse("06:30"), End = TimeSpan.Parse("06:59"), Amount = 13 },
                    new TaxRule { Start = TimeSpan.Parse("07:00"), End = TimeSpan.Parse("07:59"), Amount = 18 },
                    new TaxRule { Start = TimeSpan.Parse("08:00"), End = TimeSpan.Parse("08:29"), Amount = 13 },
                    new TaxRule { Start = TimeSpan.Parse("08:30"), End = TimeSpan.Parse("14:59"), Amount = 8 },
                    new TaxRule { Start = TimeSpan.Parse("15:00"), End = TimeSpan.Parse("15:29"), Amount = 13 },
                    new TaxRule { Start = TimeSpan.Parse("15:30"), End = TimeSpan.Parse("16:59"), Amount = 18 },
                    new TaxRule { Start = TimeSpan.Parse("17:00"), End = TimeSpan.Parse("17:59"), Amount = 13 },
                    new TaxRule { Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("18:29"), Amount = 8 },
                    new TaxRule { Start = TimeSpan.Parse("18:30"), End = TimeSpan.Parse("23:59"), Amount = 0 },
                    new TaxRule { Start = TimeSpan.Parse("00:00"), End = TimeSpan.Parse("05:59"), Amount = 0 }
                ],
                Holidays =
                [
                    new Holiday { Date = new DateTime(2013, 1, 1) },
                    new Holiday { Date = new DateTime(2013, 3, 29) },
                    new Holiday { Date = new DateTime(2013, 4, 1) },
                    new Holiday { Date = new DateTime(2013, 5, 1) },
                    new Holiday { Date = new DateTime(2013, 5, 9) },
                    new Holiday { Date = new DateTime(2013, 6, 6) },
                    new Holiday { Date = new DateTime(2013, 6, 21) },
                    new Holiday { Date = new DateTime(2013, 12, 25) },
                    new Holiday { Date = new DateTime(2013, 12, 26) }

                ],
                ExemptVehicles = [VehicleType.Emergency, VehicleType.Bus, VehicleType.Military, VehicleType.Motorcycle, VehicleType.Diplomat, VehicleType.Foreign],
                ExemptedMonths = [7]
            };

            // Define Gothenburg city with its rule set
            var gothenburg = new CityConfig
            {
                CityName = "Gothenburg",
                RuleSet = gothenburgRuleSet
            };

            db.Cities.Add(gothenburg);
            db.SaveChanges();
        }
    }
}
