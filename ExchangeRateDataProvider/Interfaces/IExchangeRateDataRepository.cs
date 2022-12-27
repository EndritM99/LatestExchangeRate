using ExchangeRateDataProvider.Models;

namespace ExchangeRateDataProvider.Interfaces
{
    public interface IExchangeRateDataRepository
    {
        public Task<FixerDataResponse> GetLatestExchangeRate();
    }
}
