using LoanProcessing.Application.Common.Exceptions;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication;
using LoanProcessing.Application.LoanApplication.Commands.EditLoanApplication;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList;
public class GetLoanApplicationsQueryHandler : IRequestHandler<GetLoanApplicationsQuery, GetLoanApplicationsQueryResponse>
{
    private readonly ILoanApplicationRepository _repository;
    private readonly ILogger<GetLoanApplicationsQueryHandler> _logger;

    public GetLoanApplicationsQueryHandler(ILoanApplicationRepository repository, ILogger<GetLoanApplicationsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetLoanApplicationsQueryResponse> Handle(GetLoanApplicationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Started {nameof(GetLoanApplicationsQueryHandler)}");
        var applications = await _repository.GetAllByUserIdAsync(request.UserId);

        if (applications == null || !applications.Any())
            return new GetLoanApplicationsQueryResponse();

        var response = new GetLoanApplicationsQueryResponse
        {
            apps = applications.Select(app => new AppList
            {
                Id = app.Id,
                LoanType = app.LoanType,
                Amount = app.Amount,
                Currency = app.Currency,
                Period = app.Period,
                Status = app.Status
            }).ToList()
        };

        _logger.LogInformation($"Finished {nameof(GetLoanApplicationsQueryHandler)}");
        return response;
    }
}

