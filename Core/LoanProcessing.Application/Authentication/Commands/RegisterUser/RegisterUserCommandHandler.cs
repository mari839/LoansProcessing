using LoanProcessing.Application.Helpers;
using LoanProcessing.Application.Interfaces.Authnetication;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.Shared.Authentication;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.Authentication.Commands.RegisterUser;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator; 
    }

    public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Started {nameof(RegisterUserCommandHandler)}");

        if (await _userRepository.ExistsAsync(request.Username))
            throw new InvalidOperationException("Username already exists.");

        var passwordHash = HashingHelper.ComputeSha256Hash(request.Password);

        var user = User.Create(
            request.Username,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.PersonalId,
            request.BirthDate,
            Role.User
        );

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user); 

        _logger.LogInformation($"Finished {nameof(RegisterUserCommandHandler)}");

        return new RegisterResponseDto
        {
            UserId = user.Id,
            Token = token
        };
    }
}
