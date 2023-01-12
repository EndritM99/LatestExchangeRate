using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IConsumerService
    {
        public FixerRestClientResponse ReadMessage();
    }
}
