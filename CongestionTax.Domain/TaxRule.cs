using CongestionTax.Domain;

public class TaxRule
{
    public int Id { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public int Amount { get; set; }
    public int RuleSetId { get; set; }
}

