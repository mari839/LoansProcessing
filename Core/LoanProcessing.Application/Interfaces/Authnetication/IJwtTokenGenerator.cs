using LoanProcessing.Domain.Entities;

namespace LoanProcessing.Application.Interfaces.Authnetication;
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

