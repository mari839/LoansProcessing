using LoanProcessing.Domain.Enums;
using MediatR;

namespace LoanProcessing.Application.LoanApplication.Commands.EditLoanApplication;
public record EditLoanApplicationCommand(
    long Id,
    decimal Amount,
    string Currency,
    int Period,
    LoanType LoanType) : IRequest<long>;

