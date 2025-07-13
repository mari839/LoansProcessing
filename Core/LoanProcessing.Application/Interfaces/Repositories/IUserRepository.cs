using LoanProcessing.Domain.Entities;

namespace LoanProcessing.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string username);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<User?> GetUserByIdAsync(long id);  
    }
}
