using MediatR;
namespace LoanProcessing.Application.Shared.Authentication;
public abstract record RegisterCommandBase(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string PersonalId,
    DateTime BirthDate
) : IRequest<RegisterResponseDto>;

