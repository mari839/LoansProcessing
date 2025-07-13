using LoanProcessing.Application.Helpers;
using LoanProcessing.Application.Interfaces.Authnetication;
using LoanProcessing.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ILogger<LoginCommandHandler> logger,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started {Handler} with Username: {Username}", nameof(LoginCommandHandler), request.Username);

            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username");

            var hashedInput = HashingHelper.ComputeSha256Hash(request.Password);

            if (user.PasswordHash != hashedInput)
                throw new UnauthorizedAccessException("Invalid password.");

            var token = _jwtTokenGenerator.GenerateToken(user);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                UserId = user.Id
            };
            _logger.LogInformation("Login response: {@Response}", response);
            return response;
        }
    }
}
