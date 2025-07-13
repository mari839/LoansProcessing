using LoanProcessing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanProcessing.Persistence.ApplicationDbContext
{
    public class LoanProcessingDbContext : DbContext
    {
        public LoanProcessingDbContext(DbContextOptions<LoanProcessingDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanProcessingDbContext).Assembly);
        }
    }
}
