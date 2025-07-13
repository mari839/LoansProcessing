using LoanProcessing.Application.Interfaces.Messaging;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Commands.SubmitLoanApplication
{
    public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand, long>
    {
        private readonly ILoanApplicationRepository _repository;
        private readonly IRabbitMqProducer _producer;
        private readonly ILogger<SubmitLoanApplicationCommandHandler> _logger;

        public SubmitLoanApplicationCommandHandler(ILoanApplicationRepository repository, IRabbitMqProducer producer, ILogger<SubmitLoanApplicationCommandHandler> logger)
        {
            _repository = repository;
            _producer = producer;
            _logger = logger;
        }

        public async Task<long> Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started {nameof(SubmitLoanApplicationCommandHandler)}, Id {request.Id}");

            var application = await _repository.GetByIdAsync(request.Id);

            if (application is null)
                throw new KeyNotFoundException($"LoanApplication {request.Id} not found.");

            application.Submit();

            await _repository.SaveChangesAsync(cancellationToken);

            try
            {
                _producer.PublishLoan(application); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish loan to RabbitMQ");
            }

            _logger.LogInformation($"Finished {nameof(SubmitLoanApplicationCommandHandler)}");


            return application.Id;
        }
    }
}
