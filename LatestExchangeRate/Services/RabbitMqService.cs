using LatestExchangeRate.Models;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using LatestExchangeRate.Interfaces;

namespace LatestExchangeRate.Services
{
    public class RabbitMqService : IRabbitMq
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;

        public RabbitMqService(ConnectionFactory factory)
        {
            _factory = factory;
            _connection = _factory.CreateConnection();
        }

        public void SendMessage(FixerRestClientResponse response)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Queue exchange-rate-response-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = JsonConvert.SerializeObject(response);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "Queue exchange-rate-response-queue", basicProperties: null, body: body);
            }
        }

        public FixerRestClientResponse ReceiveMessage()
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Queue exchange-rate-response-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                FixerRestClientResponse response = null;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    response = JsonConvert.DeserializeObject<FixerRestClientResponse>(message);
                };

                channel.BasicConsume(queue: "Queue exchange-rate-response-queue", autoAck: true, consumer: consumer);

                return response;
            }
        }
    }
}
