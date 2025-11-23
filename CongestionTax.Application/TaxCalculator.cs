using CongestionTax.Domain;
using CongestionTax.Domain.Interfaces;

namespace CongestionTax.Application;

public class TaxCalculator : ITaxCalculator
{
    public int CalculateTax(Vehicle vehicle, List<DateTime> timestamps, RuleSet ruleSet)
    {
        if (vehicle == null || timestamps == null || ruleSet == null)
            throw new ArgumentNullException("Arguments cannot be null");

        // Filter out exempt vehicles
        if (ruleSet.ExemptVehicles.Contains(vehicle.Type))
            return 0;

        // Group timestamps into single charge based on 60-minute window
        var groupedTimestamps = GroupTimestampsBySingleCharge(timestamps, ruleSet.SingleChargeMinutes);

        // Calculate the total tax
        int totalTax = 0;

        foreach (var group in groupedTimestamps)
        {
            // Calculate the maximum tax amount for the group
            int maxTax = 0;
            foreach (var timestamp in group)
            {
                var tax = CalculateTaxForTimestamp(timestamp, ruleSet);
                maxTax = Math.Max(maxTax, tax);
            }
            totalTax += maxTax;
        }

        // Ensure total tax does not exceed the max daily fee
        return Math.Min(totalTax, ruleSet.MaxDailyFee);
    }

    private List<List<DateTime>> GroupTimestampsBySingleCharge(List<DateTime> timestamps, int chargeWindowMinutes)
    {
        var grouped = new List<List<DateTime>>();
        timestamps = [.. timestamps.OrderBy(t => t)];

        List<DateTime> currentGroup = [];
        foreach (var timestamp in timestamps)
        {
            if (currentGroup.Count == 0 || (timestamp - currentGroup.Last()).TotalMinutes <= chargeWindowMinutes)
            {
                currentGroup.Add(timestamp);
            }
            else
            {
                grouped.Add([.. currentGroup]);
                currentGroup.Clear();
                currentGroup.Add(timestamp);
            }
        }

        if (currentGroup.Any())
            grouped.Add(currentGroup);

        return grouped;
    }

    private int CalculateTaxForTimestamp(DateTime timestamp, RuleSet ruleSet)
    {
        if (ruleSet.ExemptedMonths.Contains(timestamp.Date.Month))
            return 0;

        var holiday = ruleSet.Holidays.Any(h => h.Date.Date == timestamp.Date);
        if (holiday)
            return 0; // No tax on holidays
        
        if (IsDayBeforeHoliday(timestamp, ruleSet.Holidays))
            return 0; // No tax on days before holidays

        if (IsWeekend(timestamp) || IsDayBeforeHoliday(timestamp, ruleSet.Holidays))
            return 0; // No tax on weekends

        // Find applicable tax rule based on the time of day
        var taxRule = ruleSet.TaxRules.FirstOrDefault(rule => timestamp.TimeOfDay >= rule.Start && timestamp.TimeOfDay <= rule.End);
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
