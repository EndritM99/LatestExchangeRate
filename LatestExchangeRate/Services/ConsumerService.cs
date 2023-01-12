using LatestExchangeRate.Constans;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LatestExchangeRate.Services
{
    public class ConsumerService : IConsumerService
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
            FixerRestClientResponse response = null;
            while (response == null)
            {
                var queueDeclareOk = _model.QueueDeclarePassive(RestClientConstants.QueueName);
                if (queueDeclareOk.MessageCount > 0)
                {
                    var basicGetResult = _model.BasicGet(RestClientConstants.QueueName, true);
                    if (basicGetResult == null)
                        continue;
                    var body = basicGetResult.Body.ToArray();
                    var text = System.Text.Encoding.UTF8.GetString(body);
                    response = JsonConvert.DeserializeObject<FixerRestClientResponse>(text);
                    //_model.BasicAck(basicGetResult.DeliveryTag, false);
                }
                else
                {   
                    Thread.Sleep(1000);
                }
            }
            return response;
        }
    }
}
