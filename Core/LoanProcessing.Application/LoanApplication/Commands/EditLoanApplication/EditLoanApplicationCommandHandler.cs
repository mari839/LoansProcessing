using LoanProcessing.Application.Common.Exceptions;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Commands.EditLoanApplication
{
    public class EditLoanApplicationCommandHandler : IRequestHandler<EditLoanApplicationCommand, long>
    {
        private readonly ILoanApplicationRepository _repository;
        private readonly ILogger<EditLoanApplicationCommandHandler> _logger;

        public EditLoanApplicationCommandHandler(ILoanApplicationRepository repository, ILogger<EditLoanApplicationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<long> Handle(EditLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started {nameof(EditLoanApplicationCommandHandler)}");
            var application = await _repository.GetByIdAsync(request.Id);
            if (application is null)
                throw new NotFoundException(nameof(LoanApplication), request.Id);

            application.Edit(request.Amount, request.Currency, request.Period, request.LoanType);

            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Finished {nameof(EditLoanApplicationCommandHandler)}");

            return application.Id;
        }
    }
}
