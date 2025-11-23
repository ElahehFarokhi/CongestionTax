namespace CongestionTax.Domain.Interfaces
{
    public interface ITaxCalculator
    {
        int CalculateTax(Vehicle vehicle, List<DateTime> dates, RuleSet rules);
    }

}
