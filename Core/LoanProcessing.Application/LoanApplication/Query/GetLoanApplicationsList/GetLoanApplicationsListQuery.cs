using MediatR;

namespace LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList;

public record GetLoanApplicationsListQuery() :IRequest<List<GetLoanApplicationsListQueryResponse>>;

