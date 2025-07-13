using FluentValidation;
using LoanProcessing.Application.Authentication.Commands.RegisterUser;
namespace LoanProcessing.Application.Authentication.Commands.RegisterApprover;

public class RegisterApproverCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterApproverCommandValidator()
    {
        RuleFor(x => x.PersonalId)
            .NotEmpty().WithMessage("Personal ID is required.")
            .Matches(@"^\d+$").WithMessage("Personal ID must contain digits only.");
    }
}
