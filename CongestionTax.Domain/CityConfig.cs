namespace CongestionTax.Domain;

public class CityConfig
{
    public int Id { get; set; }
    public string CityName { get; set; } = "";
            
    public int RuleSetId { get; set; }
    public RuleSet RuleSet { get; set; }
}
