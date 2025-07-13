namespace LoanProcessing.Application.Interfaces.Messaging;
public interface IRabbitMqConsumer
{
    void ConsumeLoanApplications(CancellationToken cancellationToken);
}

