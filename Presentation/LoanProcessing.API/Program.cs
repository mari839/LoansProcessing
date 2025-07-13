using FluentValidation.AspNetCore;
using LoanProcessing.Application;
using LoanProcessing.Application.Interfaces.Messaging;
using LoanProcessing.Infrastructure;
using LoanProcessing.Persistence;
using LoanProcessing.Persistence.ApplicationDbContext;
using LoanProcessing.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341") 
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend",
        policy => policy
            .WithOrigins("https://localhost:44373") // your frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());  // if you need cookies/auth
});


builder.Services.AddApplicationService(builder.Configuration); 
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers()
    .AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
    });


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoanProcessing API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token only. 'Bearer' will be added automatically.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer",              
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new string[] { }
        }
    });
});




var app = builder.Build();


await DatabaseSeeder.SeedDefaultAdminAsync(app.Services);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoanProcessingDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowLocalhostFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

