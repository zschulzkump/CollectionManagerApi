using Amazon.Runtime;
using Amazon.S3;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using CollectionManagerApi;
using CollectionManagerApi.Database;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = Environment.GetEnvironmentVariable("RUSTFS_S3_ENDPOINT"),
        ForcePathStyle = true, // Required for many S3-compatible local stores
        AuthenticationRegion = "us-east-1"
    };
    var s3Client = new AmazonS3Client(Environment.GetEnvironmentVariable("RUSTFS_S3_ACCESSKEY"), Environment.GetEnvironmentVariable("RUSTFS_S3_SECRETKEY"), config);
    
    return s3Client;
});

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();

    })
    .AddMvc(options => options.Conventions.Add(new VersionByNamespaceConvention()))
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Collection Manager API")
            .ShowOperationId()
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.AddDocument("v1", "Collection Manager API Server v1", "/openapi/v1.json", isDefault: true);
    });
    
    await using var serviceScope = app.Services.CreateAsyncScope();
    await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
    
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }

    if (!await roleManager.RoleExistsAsync(Roles.User))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.User));
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();