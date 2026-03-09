using Exam.App.Domain;
using Exam.App.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace Exam.App;

public static class Startup
{
    public static void AddLogging(WebApplicationBuilder builder)
    {
        Console.WriteLine("AddLogging: START");
        try
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            Console.WriteLine("AddLogging: SUCCESS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddLogging: ERROR - {ex.Message}");
            throw;
        }
    }

    public static void AddCors(WebApplicationBuilder webApplicationBuilder)
    {
        Console.WriteLine("AddCors: START");
        try
        {
            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                    });
            });
            Console.WriteLine("AddCors: SUCCESS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddCors: ERROR - {ex.Message}");
            throw;
        }
    }

    public static void AddSwagger(WebApplicationBuilder builder)
    {
        Console.WriteLine("AddSwagger: START");
        try
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Building Example API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insert JWT token"
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
                        []
                    }
                });
            });
            Console.WriteLine("AddSwagger: SUCCESS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddSwagger: ERROR - {ex.Message}");
            throw;
        }
    }

    public static void AddAuthenticationAndAuthorization(WebApplicationBuilder builder)
    {
        Console.WriteLine("AddAuthenticationAndAuthorization: START");
        try
        {
            Console.WriteLine("AddAuthenticationAndAuthorization: Adding Identity...");
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                 .AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders();
            Console.WriteLine("AddAuthenticationAndAuthorization: Identity added");

            Console.WriteLine("AddAuthenticationAndAuthorization: Configuring password...");
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            });
            Console.WriteLine("AddAuthenticationAndAuthorization: Password configured");

            Console.WriteLine("AddAuthenticationAndAuthorization: Adding Authentication...");
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            Console.WriteLine($"JWT Key present: {!string.IsNullOrEmpty(jwtKey)}");
            Console.WriteLine($"JWT Issuer: {jwtIssuer}");
            Console.WriteLine($"JWT Audience: {jwtAudience}");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        RoleClaimType = ClaimTypes.Role
                    };
                });
            Console.WriteLine("AddAuthenticationAndAuthorization: Authentication added");

            Console.WriteLine("AddAuthenticationAndAuthorization: Adding Authorization...");
            builder.Services.AddAuthorization();
            Console.WriteLine("AddAuthenticationAndAuthorization: SUCCESS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddAuthenticationAndAuthorization: ERROR - {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
            throw;
        }
    }
}
