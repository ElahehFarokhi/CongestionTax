
namespace CongestionTax.Domain;

public class RuleSet
{
    public int Id { get; set; }
    public int MaxDailyFee { get; set; }
    public int SingleChargeMinutes { get; set; }

    public List<TaxRule> TaxRules { get; set; } = [];
    public List<VehicleType> ExemptVehicles { get; set; } = [];
    public List<Holiday> Holidays { get; set; } = [];
    public List<int> ExemptedMonths { get; set; } = [];

}
