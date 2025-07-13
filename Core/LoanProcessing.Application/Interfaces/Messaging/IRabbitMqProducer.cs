namespace LoanProcessing.Application.Interfaces.Messaging;

public interface IRabbitMqProducer
{
    void PublishLoan(object message);
    //void Send<T>(T message);
}

