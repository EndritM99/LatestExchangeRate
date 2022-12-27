using LatestExchangeRate.Constans;
using LatestExchangeRate.Extensions;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using LatestExchangeRate.Context;
using Newtonsoft.Json;
using RestSharp;

namespace LatestExchangeRate.Services
{
    public class FixerService : IExchangeRate
    {
        private readonly AppDbContext _context;

        public FixerService(AppDbContext context)
        {
            _context = context;
        }

        public FixerRestClientResponse GetLatestExchangeRate(FixerRestClientRequest fixerRestClientRequest)
        {
            if (fixerRestClientRequest == null)
            {
                throw new Exception("...");
            }  

            var client = new RestClient(RestClientConstants.BaseURL);
            var request = new RestRequest(RestClientConstants.LatestRateEndpoint, Method.Get);

            request.AddHeader("apikey", RestClientConstants.ApiKey);
            request.AddParameter("symbols", fixerRestClientRequest.Symbols, ParameterType.QueryString);
            request.AddParameter("base", fixerRestClientRequest.Base, ParameterType.QueryString);

            var response = client.Execute(request);

            var jsonResponse = response.Content;
            var result = JsonConvert.DeserializeObject<FixerRestClientResponse>(jsonResponse);

            SaveResponseToDatabase(result);
            return result;
        }

        public void SaveResponseToDatabase(FixerRestClientResponse response)
        {
            var entity = response.ToEntity();

            _context.FixerResponses.Add(entity);
            _context.SaveChanges();
        }
    }
}
