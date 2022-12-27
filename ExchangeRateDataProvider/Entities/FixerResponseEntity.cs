using ExchangeRateDataProvider.Models;

namespace ExchangeRateDataProvider.Entities
{
    public class FixerResponseEntity
    {
        public int Id { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public long Timestamp { get; set; }
        public ICollection<CurrencyRate> CurrencyRates { get; set; }
    }
}
