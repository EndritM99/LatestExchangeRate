using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateUpdate
    {
        public Task<LatestExchangeRateResponse> ExchangeRateUpdateServiceAsync(LatestExchangeRateRequest latestExchangeRateRequest);
    }
}
