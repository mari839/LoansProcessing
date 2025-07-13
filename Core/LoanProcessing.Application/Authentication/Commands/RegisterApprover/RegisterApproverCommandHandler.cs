using LoanProcessing.Application.Authentication.Commands.RegisterUser;
using LoanProcessing.Application.Helpers;
using LoanProcessing.Application.Interfaces.Authnetication;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Application.Shared.Authentication;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanProcessing.Application.Authentication.Commands.RegisterApprover;
public class RegisterApproverCommandHandler : IRequestHandler<RegisterApproverCommand, RegisterResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RegisterApproverCommandHandler> _logger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public RegisterApproverCommandHandler(IUserRepository userRepository, ILogger<RegisterApproverCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<RegisterResponseDto> Handle(RegisterApproverCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Started {nameof(RegisterApproverCommandHandler)}");

        if (await _userRepository.ExistsAsync(request.Username))
            throw new InvalidOperationException("Username already exists.");

        var passwordHash = HashingHelper.ComputeSha256Hash(request.Password);

        var approver = User.Create(
            request.Username,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.PersonalId,
            request.BirthDate,
            Role.Approver
        );

        await _userRepository.AddAsync(approver);
        await _userRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Finished {nameof(RegisterApproverCommandHandler)}");

        var token = _jwtTokenGenerator.GenerateToken(approver);

        return new RegisterResponseDto
        {
            UserId = approver.Id,
            Token = token
        };
    }
}

