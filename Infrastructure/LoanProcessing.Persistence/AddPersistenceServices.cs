using LoanProcessing.Application.Interfaces.Repositories;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Persistence.ApplicationDbContext;
using LoanProcessing.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanProcessing.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LoanProcessingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

            return services;
        }
    }
}
