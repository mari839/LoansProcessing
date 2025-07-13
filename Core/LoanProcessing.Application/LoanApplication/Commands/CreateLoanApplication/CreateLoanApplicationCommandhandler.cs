using LoanProcessing.Application.Authentication.Commands.Login;
using LoanProcessing.Application.Authentication.Commands.RegisterUser;
using LoanProcessing.Application.Helpers;
using LoanProcessing.Application.Interfaces.Messaging;
using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LoanProcessing.Application.LoanApplication.Commands.CreateLoanApplication
{
    public class CreateLoanApplicationCommandhandler : IRequestHandler<CreateLoanApplicationCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRabbitMqProducer _producer;
        private readonly ILogger<CreateLoanApplicationCommandhandler> _logger;
        public CreateLoanApplicationCommandhandler(IUserRepository userRepository, IRabbitMqProducer producer, ILogger<CreateLoanApplicationCommandhandler> logger)
        {
            _userRepository = userRepository;
            _producer = producer;
            _logger = logger;
        }

        public async Task<long> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started {Handler} with request: {request}", nameof(CreateLoanApplicationCommand), request.AsJson());

            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var application = new Domain.Entities.LoanApplication
            {
                LoanType = request.LoanType,
                Amount = request.Amount,
                Currency = request.Currency,
                Period = request.Period,
                Status = LoanApplicaitonStatus.InProcess,
                UserId = request.UserId
            };

            user.CreateLoanApplication(application); 

            await _userRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Finished {nameof(CreateLoanApplicationCommandhandler)}");

            return application.Id;
        }
    }
}
