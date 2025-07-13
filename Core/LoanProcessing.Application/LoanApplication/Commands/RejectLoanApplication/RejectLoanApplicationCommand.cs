using MediatR;

namespace LoanProcessing.Application.LoanApplication.Commands.DeleteLoanApplication;

public record RejectLoanApplicationCommand(long Id) : IRequest<long>;

