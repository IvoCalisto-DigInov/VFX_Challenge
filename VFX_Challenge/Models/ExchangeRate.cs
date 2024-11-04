namespace VFX_Challenge.Models
{
    public class ExchangeRate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string BaseCurrency { get; set; } = string.Empty;
        public string QuoteCurrency { get; set; } = string.Empty;
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
