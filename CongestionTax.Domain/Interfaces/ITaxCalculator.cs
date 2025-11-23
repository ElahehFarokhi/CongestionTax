namespace CongestionTax.Domain.Interfaces
{
    public interface ITaxCalculator
    {
        Dictionary<DateTime, int> CalculateTax(Vehicle vehicle, List<DateTime> dates, RuleSet rules);
    }

}
