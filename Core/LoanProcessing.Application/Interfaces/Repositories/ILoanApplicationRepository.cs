namespace LoanProcessing.Application.Interfaces.Repositories;
public interface ILoanApplicationRepository
{
    Task<Domain.Entities.LoanApplication?> GetByIdAsync(long id);
    Task AddAsync(Domain.Entities.LoanApplication loanApplication);
    Task<IEnumerable<Domain.Entities.LoanApplication>> GetAllByUserIdAsync(long userId);
    Task<IEnumerable<Domain.Entities.LoanApplication>> GetAllLoanApplicationsAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}

