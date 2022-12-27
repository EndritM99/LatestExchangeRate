using ExchangeRateDataProvider.Entities;

namespace ExchangeRateDataProvider.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
        public int FixerResponseEntityId { get; set; }
        public FixerResponseEntity FixerResponseEntity { get; set; }
    }
}
