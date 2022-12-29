using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IJobScheduler
    {
        public string EnqueueGetLatestExchangeRate(FixerRestClientRequest request);
    }
}
