using LoanProcessing.Application.Authentication.Commands.Login;
using LoanProcessing.Application.Common.Exceptions;
using LoanProcessing.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Commands.ApproveLoanApplication;
    public class ApproveLoanApplicationCommandHandler : IRequestHandler<ApproveLoanApplicationCommand, long>
    {
        private readonly ILoanApplicationRepository _repository;
        private readonly ILogger<ApproveLoanApplicationCommandHandler> _logger;

        public ApproveLoanApplicationCommandHandler(ILoanApplicationRepository repository, ILogger<ApproveLoanApplicationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<long> Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started {Handler} with id: {Id}", nameof(ApproveLoanApplicationCommandHandler), request.Id);

            var application = await _repository.GetByIdAsync(request.Id);
            if (application is null)
                throw new NotFoundException(nameof(LoanApplication), request.Id);

            application.Approve();

            await _repository.SaveChangesAsync(cancellationToken);
            return application.Id;
        }
    }

