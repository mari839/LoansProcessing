using LoanProcessing.Application.Interfaces.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;

namespace LoanProcessing.Infrastructure.RabbitMq
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly RabbitMqSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqConsumer(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;

            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _settings.LoanQueue,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void ConsumeLoanApplications(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var application = JsonSerializer.Deserialize<LoanApplication>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (application is not null && application.Status == LoanApplicaitonStatus.Submitted)
                    {
                        Console.WriteLine($"Received Submitted Loan: ID={application.Id}, Amount={application.Amount}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: _settings.LoanQueue,
                                  autoAck: false,
                                  consumer: consumer);
        }
    }
}
