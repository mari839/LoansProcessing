using FluentValidation;

namespace LoanProcessing.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.PersonalId)
                .NotEmpty().WithMessage("Personal ID is required.")
                .Matches(@"^\d+$").WithMessage("Personal ID must contain digits only.");
        }
    }
}
