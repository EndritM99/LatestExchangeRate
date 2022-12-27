namespace ExchangeRateUpdater.Models
{
    public class LatestExchangeRateResponse
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }
        public bool Success { get; set; }
        public long Timestamp { get; set; }
    }
}
