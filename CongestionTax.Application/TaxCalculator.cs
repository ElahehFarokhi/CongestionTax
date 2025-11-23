using CongestionTax.Domain;
using CongestionTax.Domain.Interfaces;

namespace CongestionTax.Application;

public class TaxCalculator : ITaxCalculator
{
    public Dictionary<DateTime, int> CalculateTax(Vehicle vehicle, List<DateTime> timestamps, RuleSet ruleSet)
    {
        if (vehicle == null || timestamps == null || ruleSet == null)
            throw new ArgumentNullException("Arguments cannot be null");

        // Filter out exempt vehicles
        if (ruleSet.ExemptVehicles.Contains(vehicle.Type))
            return new Dictionary<DateTime, int>();

        // Group timestamps by day
        var groupedByDay = timestamps
            .GroupBy(t => t.Date)
            .OrderBy(g => g.Key);

        var result = new Dictionary<DateTime, int>();


        foreach (var dayGroup in groupedByDay)
        {
            var groupedTimestamps = GroupTimestampsBySingleCharge(dayGroup.ToList(), ruleSet.SingleChargeMinutes);

            int dailyTotal = 0;
            foreach (var group in groupedTimestamps)
            {
                int maxTax = 0;
                foreach (var timestamp in group)
                {
                    var tax = CalculateTaxForTimestamp(timestamp, ruleSet);
                    maxTax = Math.Max(maxTax, tax);
                }
                dailyTotal += maxTax;
            }

            // Apply daily cap
            result[dayGroup.Key] = Math.Min(dailyTotal, ruleSet.MaxDailyFee);
        }

        return result;
    }

    private List<List<DateTime>> GroupTimestampsBySingleCharge(List<DateTime> timestamps, int chargeWindowMinutes)
    {
        var grouped = new List<List<DateTime>>();
        timestamps = timestamps.OrderBy(t => t).ToList();

        List<DateTime> currentGroup = new();
        DateTime? windowStart = null;

        foreach (var timestamp in timestamps)
        {
            if (currentGroup.Count == 0)
            {
                currentGroup.Add(timestamp);
                windowStart = timestamp;
            }
            else if ((timestamp - windowStart.Value).TotalMinutes <= chargeWindowMinutes)
            {
                currentGroup.Add(timestamp);
            }
            else
            {
                grouped.Add(new List<DateTime>(currentGroup));
                currentGroup.Clear();
                currentGroup.Add(timestamp);
                windowStart = timestamp;
            }
        }

        if (currentGroup.Any())
            grouped.Add(currentGroup);

        return grouped;
    }

    private int CalculateTaxForTimestamp(DateTime timestamp, RuleSet ruleSet)
    {
        // Month exemption
        if (ruleSet.ExemptedMonths.Contains(timestamp.Month))
            return 0;

        // Holiday check
        if (ruleSet.Holidays.Any(h => h.Date.Date == timestamp.Date))
            return 0;

        // Weekend / day before holiday
        if (IsWeekend(timestamp) || IsDayBeforeHoliday(timestamp, ruleSet.Holidays))
            return 0;

        // Find applicable tax rule
        var taxRule = ruleSet.TaxRules
            .FirstOrDefault(rule => timestamp.TimeOfDay >= rule.Start && timestamp.TimeOfDay <= rule.End);

        return taxRule?.Amount ?? 0;
    }

    private bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private bool IsDayBeforeHoliday(DateTime date, List<Holiday> holidays)
    {
        return holidays.Any(h => h.Date.Date == date.AddDays(1).Date);
    }
}
