using Newtonsoft.Json;

namespace ExchangeRateDataProvider.Models
{
    public class FixerDataResponse
    {

        [JsonProperty("base")]
        public string? Base { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double>? Rates { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
