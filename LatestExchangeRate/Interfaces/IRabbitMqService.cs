using RabbitMQ.Client;

namespace LatestExchangeRate.Interfaces
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}
