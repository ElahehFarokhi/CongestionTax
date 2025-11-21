using CongestionTax.Domain;

public class TaxCalculator : ITaxCalculator
{
    public int CalculateTax(Vehicle vehicle, List<DateTime> dates, RuleSet rules)
    {
        if (rules.ExemptVehicles.Contains(vehicle.Type)) return 0;

        var filtered = dates
            .Where(dt => IsChargeable(dt, rules))
            .OrderBy(x => x)
            .ToList();

        return ApplySingleChargeRule(filtered, rules);
    }

    private bool IsChargeable(DateTime dt, RuleSet rules)
    {
        if (dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return false;
        if (rules.ApplyJulyExemption && dt.Month == 7) return false;
        if (rules.Holidays.Any(h => h.Date.Date == dt.Date)) return false;
        if (rules.Holidays.Any(h => h.Date.AddDays(-1).Date == dt.Date)) return false;
        return true;
    }

    private int ApplySingleChargeRule(List<DateTime> times, RuleSet rules)
    {
        if (!times.Any()) return 0;

        DateTime intervalStart = times[0];
        int maxFee = GetFee(times[0], rules);

        List<int> results = new();

        foreach (var t in times.Skip(1))
        {
            if ((t - intervalStart).TotalMinutes <= rules.Config.SingleChargeMinutes)
            {
                maxFee = Math.Max(maxFee, GetFee(t, rules));
            }
            else
            {
                results.Add(maxFee);
                intervalStart = t;
                maxFee = GetFee(t, rules);
            }
        }

        results.Add(maxFee);
        return Math.Min(results.Sum(), rules.Config.MaxDailyFee);
    }

    private int GetFee(DateTime dt, RuleSet rules)
    {
        return rules.TaxRules
            .Where(r => dt.TimeOfDay >= r.Start && dt.TimeOfDay <= r.End)
            .Select(r => r.Amount)
            .DefaultIfEmpty(0)
            .First();
    }
}
