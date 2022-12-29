using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateUpdate
    {
        public Task<OperationResponse> ExchangeRateUpdateServiceAsync(LatestExchangeRateRequest latestExchangeRateRequest);
    }
}
