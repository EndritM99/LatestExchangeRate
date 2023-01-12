using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IResponsePublisher
    {
        public void SendMessage(FixerRestClientResponse fixerRestClientResponse);
    }
}
