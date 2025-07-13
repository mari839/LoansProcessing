using Azure.Core;
using LoanProcessing.Application.Helpers;
using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;
using LoanProcessing.Persistence.ApplicationDbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoanProcessing.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LoanProcessingDbContext>();

        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync(u => u.Role == Role.Admin))
        {

            var admin = User.Create(
                username: "Admin",
                passwordHash: HashingHelper.ComputeSha256Hash("Admin"),
                firstName: "Admin",
                lastName: "User",
                personalId: "12345678901",
                birthDate: new DateTime(1990, 1, 1),
                role: Role.Admin
            );

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
