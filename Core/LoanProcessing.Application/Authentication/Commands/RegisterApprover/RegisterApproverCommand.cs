using LoanProcessing.Application.Authentication.Commands.RegisterUser;
using LoanProcessing.Application.Shared.Authentication;
using MediatR;

namespace LoanProcessing.Application.Authentication.Commands.RegisterApprover;

public record RegisterApproverCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string PersonalId,
    DateTime BirthDate
) : RegisterCommandBase(Username, Password, FirstName, LastName, PersonalId, BirthDate);


