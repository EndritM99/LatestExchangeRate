using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateUpdateService : IExchangeRateUpdate
    {
        public async Task<OperationResponse> ExchangeRateUpdateServiceAsync(LatestExchangeRateRequest latestExchangeRateRequest) 
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(ExchangeRateUpdaterConstants.BaseAddres);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ExchangeRateUpdaterConstants.EndpointToHit, UriKind.Relative)
            };

            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("base", latestExchangeRateRequest?.Base),
                new KeyValuePair<string, string>("symbols", latestExchangeRateRequest?.Symbols)
            });

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            var latestExchangeRateResponse = JsonConvert.DeserializeObject<OperationResponse>(responseContent);

            return latestExchangeRateResponse;
        }
    }
}
