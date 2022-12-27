using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IJobScheduler
    {
        //public FixerRestClientResponse EnqueueGetLatestExchangeRate(FixerRestClientRequest request);

        public void EnqueueGetLatestExchangeRate(FixerRestClientRequest request);
    }
}
