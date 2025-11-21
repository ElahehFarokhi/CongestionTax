namespace CongestionTax.Domain
{
    public class CityConfig
    {
        public int Id { get; set; }
        public string CityName { get; set; } = "";
        public int MaxDailyFee { get; set; }
        public int SingleChargeMinutes { get; set; }
        public bool ApplyJulyExemption { get; set; }
    }

}
