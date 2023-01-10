using LatestExchangeRate.Constans;
using LatestExchangeRate.Extensions;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using LatestExchangeRate.Context;
using Newtonsoft.Json;
using RestSharp;
using Hangfire;
using Azure;

namespace LatestExchangeRate.Services
{
    public class FixerService : IExchangeRate
    {
        private readonly AppDbContext _context;
        private readonly RabbitMqService _rabbitMqService;

        public FixerService(AppDbContext context, RabbitMqService rabbitMqService)
        {
            _context = context;
            _rabbitMqService = rabbitMqService;
        }

        public void GetLatestExchangeRate(FixerRestClientRequest fixerRestClientRequest)
        {
            try
            {
                var client = new RestClient(RestClientConstants.BaseURL);
                var request = new RestRequest(RestClientConstants.LatestRateEndpoint, Method.Get);

                request.AddHeader("apikey", RestClientConstants.ApiKey);
                request.AddParameter("symbols", fixerRestClientRequest.Symbols, ParameterType.QueryString);
                request.AddParameter("base", fixerRestClientRequest.Base, ParameterType.QueryString);

                var response = client.Execute(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content;
                    var result = JsonConvert.DeserializeObject<FixerRestClientResponse>(jsonResponse);

                    SaveResponseToDatabase(result);
                    _rabbitMqService.SendMessage(result);
                }
                else
                {
                    throw new Exception($"Error getting the response from RestClient. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error making request to RestClient. Exception message: {ex.Message}");
            }
        }

        public void SaveResponseToDatabase(FixerRestClientResponse response)
        {
            try
            {
                var entity = response.ToEntity();

                _context.FixerResponses.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving response to database: {ex.Message}");
            }
        }
    }
}
