using MediatR;

namespace LoanProcessing.Application.LoanApplication.Commands.SubmitLoanApplication;

public record SubmitLoanApplicationCommand(long Id) : IRequest<long>;
