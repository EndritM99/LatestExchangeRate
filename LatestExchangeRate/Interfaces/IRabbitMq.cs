using LatestExchangeRate.Models;

namespace LatestExchangeRate.Interfaces
{
    public interface IRabbitMq
    {
        public void SendMessage(FixerRestClientResponse response);
        public FixerRestClientResponse ReceiveMessage();
    }
}
