using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Newtonsoft.Json;
using System.Text;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateUpdateService : IExchangeRateUpdate
    {
        public async Task<LatestExchangeRateResponse> ExchangeRateUpdateServiceAsync(LatestExchangeRateRequest latestExchangeRateRequest) 
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7089");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/latestexchangerate", UriKind.Relative)
            };

            request.Content = new StringContent(latestExchangeRateRequest?.Base, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content = new StringContent(latestExchangeRateRequest?.Symbols, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            var latestExchangeRateResponse = JsonConvert.DeserializeObject<LatestExchangeRateResponse>(responseContent);

            return latestExchangeRateResponse;
        }
    }
}
