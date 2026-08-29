using Collection_Manager_Api.Database;
using Collection_Manager_Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Collection_Manager_Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? databasePort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        string? databaseName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        string? databaseUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        string? databasePass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql($"Server=collectionmanager-db;Port={databasePort};Database={databaseName};User ID={databaseUser};Password={databasePass}");
        });
        
        return services;
    }
    
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 12;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                }
            )
            .AddEntityFrameworkStores<ApplicationDbContext>();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters.ValidIssuer = configuration["Jwt:Issuer"];
            options.TokenValidationParameters.ValidAudience = configuration["Jwt:Audience"];
            options.TokenValidationParameters.IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        });

        services.AddAuthorization();
        
        return services;
    }
}