using LatestExchangeRate.Entities;
using LatestExchangeRate.Models;

namespace LatestExchangeRate.Extensions
{
    public static class FixerResponseMappingExtensions
    {
        public static FixerResponseEntity ToEntity(this FixerRestClientResponse response)
        {

            var entity = new FixerResponseEntity
            {
                Base = response.Base,
                Date = response.Date.AddDays(1),
                Success = response.Success,
                Timestamp = response.Timestamp
            };

            entity.CurrencyRates = new List<CurrencyRate>();

            if (response.Rates != null)
            {
                foreach (var rate in response.Rates)
                {
                    entity.CurrencyRates.Add(new CurrencyRate
                    {
                        CurrencyCode = rate.Key,
                        Rate = rate.Value
                    });
                }
            }

            return entity;
        }
    }
}
