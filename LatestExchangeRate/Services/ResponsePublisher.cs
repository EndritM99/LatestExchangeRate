using Azure;
using LatestExchangeRate.Constans;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace LatestExchangeRate.Services
{
    public class ResponsePublisher : IResponsePublisher
    {
        private readonly IRabbitMqService _rabbitMqService;

        public ResponsePublisher(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public void SendMessage(FixerRestClientResponse fixerRestClientResponse)
        {
            using var connection = _rabbitMqService.CreateChannel();
            using var model = connection.CreateModel();

            string message = JsonConvert.SerializeObject(fixerRestClientResponse);

            var body = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(RestClientConstants.QueueName,
                                 string.Empty,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
