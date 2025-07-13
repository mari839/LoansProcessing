using LoanProcessing.Application.Interfaces.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Infrastructure.RabbitMq;

public class LoanApplicationConsumerService : BackgroundService
{
    private readonly IRabbitMqConsumer _consumer;
    private readonly ILogger<LoanApplicationConsumerService> _logger;

    public LoanApplicationConsumerService(IRabbitMqConsumer consumer, ILogger<LoanApplicationConsumerService> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting LoanApplicationConsumerService");
        _consumer.ConsumeLoanApplications(stoppingToken);
        return Task.CompletedTask;
    }
}

