using MediatR;

namespace LoanProcessing.Application.LoanApplication.Commands.ApproveLoanApplication;

public record ApproveLoanApplicationCommand(long Id) : IRequest<long>;

