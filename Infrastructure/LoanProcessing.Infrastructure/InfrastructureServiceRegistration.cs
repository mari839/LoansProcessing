using LoanProcessing.Application.Interfaces.Authnetication;
using LoanProcessing.Application.Interfaces.Messaging;
using LoanProcessing.Infrastructure.Auth;
using LoanProcessing.Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace LoanProcessing.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<ConfigureJwtBearerOptions>(); 
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(sp =>new ConfigureNamedOptions<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme, sp.GetRequiredService<ConfigureJwtBearerOptions>().Configure));

            services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
            services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

            services.AddHostedService<LoanApplicationConsumerService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ApproverOnly", policy => policy.RequireRole("Approver"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            return services;
        }
    }
}
