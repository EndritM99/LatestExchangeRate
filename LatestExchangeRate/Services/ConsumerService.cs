using LatestExchangeRate.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using LatestExchangeRate.Constans;
using LatestExchangeRate.Models;
using Azure;
using Newtonsoft.Json;

namespace LatestExchangeRate.Services
{
    public class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        public ConsumerService(IRabbitMqService rabbitMqService)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(RestClientConstants.QueueName, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare("ExchangeRate", ExchangeType.Fanout, durable: true, autoDelete: false);
            _model.QueueBind(RestClientConstants.QueueName, "ExchangeRate", string.Empty);
        }

        public FixerRestClientResponse ReadMessage()
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            FixerRestClientResponse response = null;

            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);

                response = JsonConvert.DeserializeObject<FixerRestClientResponse>(text);
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(RestClientConstants.QueueName, false, consumer);

            return response;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
