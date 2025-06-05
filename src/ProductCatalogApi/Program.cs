using ProductCatalogApi;
using ProductCatalogApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Services;
using ProductCatalogApi.DataAccess.Repos;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProductCatalogApi.Infrastructure;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configurationBuilder = builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = configurationBuilder.Build();

        // Add authentication and authorization services
        JwtAuthentication.SetupAuthentication(builder.Services, configuration);

        // Add services to the container.
        builder.Services.AddSingleton<IConfiguration>(configuration);
        builder.Services.AddDbContext<IProductCatalogDbContext, ProductCatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetSection("SqlServerSettings:ConnectionString").Value));
        builder.Services.AddSingleton<IAppSettings, AppSettings>();
        builder.Services.AddScoped<IProductCatalogRepo, ProductCatalogRepo>();
        builder.Services.AddScoped<ProductCatalogApi.Services.IProductsService, ProductsService>();
        

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Use scalar instead of swagger
            // app.UseSwagger();
            // app.UseSwaggerUI();

            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}