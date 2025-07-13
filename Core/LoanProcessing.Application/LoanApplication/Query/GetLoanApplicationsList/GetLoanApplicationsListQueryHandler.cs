

using LoanProcessing.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList
{
    internal class GetLoanApplicationsListQueryHandler : IRequestHandler<GetLoanApplicationsListQuery, List<GetLoanApplicationsListQueryResponse>>
    {
        private readonly ILoanApplicationRepository _repository;
        private readonly ILogger<GetLoanApplicationsListQueryHandler> _logger;

        public GetLoanApplicationsListQueryHandler(ILoanApplicationRepository repository, ILogger<GetLoanApplicationsListQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<GetLoanApplicationsListQueryResponse>> Handle(GetLoanApplicationsListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started {nameof(GetLoanApplicationsListQueryResponse)}");
            var applications = await _repository.GetAllLoanApplicationsAsync();

            if (applications == null || !applications.Any())
                return new List<GetLoanApplicationsListQueryResponse>();

            var response = applications?.Select(app => new GetLoanApplicationsListQueryResponse
            {
                Id = app.Id,
                LoanType = app.LoanType,
                Amount = app.Amount,
                Currency = app.Currency,
                Period = app.Period,
                Status = app.Status
            }).ToList() ?? new List<GetLoanApplicationsListQueryResponse>();


            _logger.LogInformation($"Finished {nameof(GetLoanApplicationsListQueryHandler)}");
            return response;
        }
    }
}
