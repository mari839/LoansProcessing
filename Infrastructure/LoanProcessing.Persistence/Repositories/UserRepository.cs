using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Persistence.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace LoanProcessing.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LoanProcessingDbContext _dbContext;
        public UserRepository(LoanProcessingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistsAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<User?> GetUserByIdAsync(long userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id == userId);
        }
    }
}
