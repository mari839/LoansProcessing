using LoanProcessing.Application.Common.Exceptions;
using LoanProcessing.Application.Helpers;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.LoanApplication.Commands.EditLoanApplication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Commands.DeleteLoanApplication;

public class RejectLoanApplicationCommandHandler : IRequestHandler<RejectLoanApplicationCommand, long>
{
    private readonly ILoanApplicationRepository _repository;
    private readonly ILogger<EditLoanApplicationCommandHandler> _logger;

    public RejectLoanApplicationCommandHandler(ILoanApplicationRepository repository, ILogger<EditLoanApplicationCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<long> Handle(RejectLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Started {nameof(RejectLoanApplicationCommandHandler)}, request {request.AsJson()}");
        var application = await _repository.GetByIdAsync(request.Id);
        if (application is null)
            throw new NotFoundException(nameof(LoanApplication), request.Id);

        application.Reject();

        await _repository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Started {nameof(RejectLoanApplicationCommandHandler)}");

        return application.Id;
    }
}


