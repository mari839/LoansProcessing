using MediatR;

namespace LoanProcessing.Application.LoanApplication.Query.GetLoanApplicationsList;

public record GetLoanApplicationsQuery(long UserId) : IRequest<GetLoanApplicationsQueryResponse>;

