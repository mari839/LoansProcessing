using LoanProcessing.Application.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace LoanProcessing.Infrastructure.RabbitMq
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly RabbitMqSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqProducer(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;

            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _settings.LoanQueue,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

        }

        public void PublishLoan(object message)
        {
            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = false
            });
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "",
                                  routingKey: _settings.LoanQueue,
                                  basicProperties: null,
                                  body: body);
        }
    }
}
