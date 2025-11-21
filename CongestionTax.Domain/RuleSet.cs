namespace CongestionTax.Domain
{
    public class RuleSet
    {
        public List<TaxRule> TaxRules { get; set; } = new();
        public List<VehicleType> ExemptVehicles { get; set; } = new();
        public List<Holiday> Holidays { get; set; } = new();
        public CityConfig Config { get; set; }
    }

}
