namespace VFX_Challenge.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string CurrencyPair { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
