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
builder.Services.AddScoped<IAppointmentService,  AppointmentService>();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}
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
app.Run();