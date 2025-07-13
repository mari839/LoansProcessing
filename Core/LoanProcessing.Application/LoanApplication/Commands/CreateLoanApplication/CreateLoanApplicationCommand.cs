using LoanProcessing.Domain.Enums;
using MediatR;

namespace LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication;

public record CreateLoanApplicationCommand(long UserId,
decimal Amount,
string Currency,
int Period,
LoanType LoanType) : IRequest<long>;

