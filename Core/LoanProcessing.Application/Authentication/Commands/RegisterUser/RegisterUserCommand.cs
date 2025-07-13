using LoanProcessing.Application.Shared.Authentication;
using LoanProcessing.Domain.Enums;
using MediatR;

namespace LoanProcessing.Application.Authentication.Commands.RegisterUser
{
    public record RegisterUserCommand(string Username,
    string Password,
    string FirstName,
    string LastName,
    string PersonalId,
    DateTime BirthDate) : RegisterCommandBase(Username, Password, FirstName, LastName, PersonalId, BirthDate);
}
