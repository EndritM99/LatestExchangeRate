using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IExchangeRate
    {
        public FixerRestClientResponse GetLatestExchangeRate(FixerRestClientRequest fixerRequest);
    }
}
