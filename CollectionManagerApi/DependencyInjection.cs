using CollectionManagerApi.Database;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;
using CollectionManagerApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CollectionManagerApi;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IS3Service, S3Service>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddScoped<ITokenProviderService, TokenProviderService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<ICollectionGroupService, CollectionGroupService>();
        services.AddTransient<IStorageService, StorageService>();
        services.AddTransient<ICollectionItemService, CollectionItemService>();
        services.AddTransient<ICustomAttributeService, CustomAttributeService>();
        services.AddTransient<ICollectionItemAttributeService, CollectionItemAttributeService>();
        services.AddTransient<ICollectionItemImageService, CollectionItemImageService>();
        services.AddTransient<ICollectionItemDocumentService, CollectionItemDocumentService>();
        services.AddTransient<ITagsService, TagsService>();
        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? databaseServer = Environment.GetEnvironmentVariable("DATABASE_SERVER");
        string? databasePort = Environment.GetEnvironmentVariable("DATABASE_PORT");
        string? databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
        string? databaseUser = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
        string? databasePass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql($"Server={databaseServer};Port={databasePort};Database={databaseName};User ID={databaseUser};Password={databasePass}");
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
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!));
        });

        services.AddAuthorization();
        
        return services;
    }
}