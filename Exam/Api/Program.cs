using Application.Interface;
using Exam.App;
using Exam.App.Controllers.Middleware;
using Exam.App.Domain;
using Exam.App.Domain.Interface;
using Exam.App.Infrastructure;
using Exam.App.Infrastructure.Database;
using Exam.App.Services;
using Exam.App.Services.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Startup.AddLogging(builder);
Startup.AddSwagger(builder);
Startup.AddCors(builder);
Startup.AddAuthenticationAndAuthorization(builder);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Service and Repository Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();

    Console.WriteLine("Starting migrations...");

    int maxRetries = 10;
    int retryCount = 0;
    while (retryCount < maxRetries)
    {
        try
        {
            Console.WriteLine("Attempting migration...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrations completed successfully!");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration attempt {retryCount + 1} failed: {ex.Message}");
            retryCount++;
            if (retryCount >= maxRetries)
            {
                Console.WriteLine("Max retries exceeded. Throwing exception.");
                throw;
            }
            await Task.Delay(1000);
        }
    }

    Console.WriteLine("Starting seed data...");
    await SeedData.InitializeAsync(services);
    Console.WriteLine("Seed data completed!");
}

// Add error handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"REQUEST ERROR: {ex}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Application starting...");
app.Run();
