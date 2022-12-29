using ExchangeRateDataProvider.Context;
using ExchangeRateDataProvider.Interfaces;
using ExchangeRateDataProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateDataProvider.Repository
{
    public class ExchangeRateDataRepository : IExchangeRateDataRepository
    {
        private readonly AppDbContext _context;

        public ExchangeRateDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FixerDataResponse> GetLatestExchangeRate()
        {
            FixerDataResponse fixerDataResponse;
            try
            {
                var fixerResponse = _context.FixerResponses
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

                var currencyRates = await _context.CurrencyRates
                    .Where(cr => cr.FixerResponseEntityId == fixerResponse.Id)
                    .ToDictionaryAsync(cr => cr.CurrencyCode, cr => cr.Rate);

                fixerDataResponse = new FixerDataResponse
                {
                    Base = fixerResponse.Base,
                    Date = fixerResponse.Date,
                    Rates = currencyRates,
                    Success = fixerResponse.Success,
                    Timestamp = fixerResponse.Timestamp
                };
            }
            catch (Exception)
            {
                fixerDataResponse = null;
                throw new Exception($"Failed to retrive data from database");
            }

            return fixerDataResponse;
        }
    }
}
