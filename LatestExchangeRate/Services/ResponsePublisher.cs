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

            model.ConfirmSelect();

            string message = JsonConvert.SerializeObject(fixerRestClientResponse);
            var body = Encoding.UTF8.GetBytes(message);
            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;
            model.BasicPublish("ExchangeRate", string.Empty, basicProperties, body);

            if (!model.WaitForConfirms())
            {
                throw new Exception("The message could not be confirmed to be written to the disk of the message broker");
            }
        }
    }
}
