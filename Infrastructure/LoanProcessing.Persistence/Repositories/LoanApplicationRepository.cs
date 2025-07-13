using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;
using LoanProcessing.Persistence.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace LoanProcessing.Persistence.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly LoanProcessingDbContext _context;

        public LoanApplicationRepository(LoanProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<LoanApplication?> GetByIdAsync(long id)
        {
            return await _context.LoanApplications
                                 .Include(l => l.User)
                                 .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
        }

        public async Task AddAsync(LoanApplication loanApplication)
        {
            await _context.LoanApplications.AddAsync(loanApplication);
        }

        public async Task<IEnumerable<LoanApplication>> GetAllByUserIdAsync(long userId)
        {
            return await _context.LoanApplications
                                 .Where(l => l.UserId == userId && !l.IsDeleted).OrderByDescending(d=>d.CreatedDate)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<LoanApplication>> GetAllLoanApplicationsAsync() 
        {
            return await _context.LoanApplications
                                 .Where(l => l.Status != LoanApplicaitonStatus.InProcess && !l.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
