using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SalesOrders;
using SalesOrders.DataAccess;
using SalesOrders.Infrastructure;
using SalesOrders.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var sqlServerSettings = configuration.GetSection("SqlServerSettings").Get<SqlServerSettings>() ?? new SqlServerSettings();

// Add services to the container.
builder.Services.AddSingleton<ISqlServerSettings>(sqlServerSettings);
builder.Services.AddDbContext<ISalesOrdersDbContext, SalesOrdersDbContext>(options =>
    options.UseSqlServer(sqlServerSettings.ConnectionString));
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddTransient<IOrderServices, OrderServices>();
builder.Services.AddTransient<IOrderMessenger, OrderMessenger>();

builder.Services.AddAutoMapper(typeof(SalesOrderProfile));
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    var entryAssembly = Assembly.GetEntryAssembly();
    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
