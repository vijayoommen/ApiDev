using ProductCatalogApi;
using ProductCatalogApi.DataAccess;
using ProductCatalogApi.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        // Add services to the container.
        builder.Services.AddSingleton<IConfiguration>(configuration);
        builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetSection("SqlServerSettings:ConnectionString").Value));
        builder.Services.AddSingleton<IAppSettings, AppSettings>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}