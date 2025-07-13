using MediatR;

namespace LoanProcessing.Application.Authentication.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
}
