using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IExchangeRate
    {
        public void GetLatestExchangeRate(FixerRestClientRequest fixerRestClientRequest);
        public void SaveResponseToDatabase(FixerRestClientResponse response);
    }
}
