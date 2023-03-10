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
            var operationResponse = new OperationResponse();

            try
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

                operationResponse = JsonConvert.DeserializeObject<OperationResponse>(responseContent);
            }
            catch (Exception ex)
            {
                operationResponse.Success = false;
                operationResponse.ErrorMessage = "An error occurred while processing the request. Please try again later";

                throw new Exception(ex.Message);
            }

            return operationResponse;
        }
    }
}
