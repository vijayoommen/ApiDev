using System.Reflection;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ShoppingSite;
using ShoppingSite.DataAccess;
using ShoppingSite.Responses;
using ShoppingSite.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var sqlServerSettings = configuration.GetSection("SqlServerSettings").Get<SqlServerSettings>() ?? new SqlServerSettings();

builder.Services.AddDbContext<IShoppingSiteDbContext, ShoppingSiteDbContext>(options =>
    options.UseSqlServer(sqlServerSettings.ConnectionString));
builder.Services.AddSingleton<ISqlServerSettings>(sqlServerSettings);

builder.Services.AddScoped<IShoppingSiteRepo, ShoppingSiteRepo>();

builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICartMessenger, CartMessenger>();

builder.Services.AddAutoMapper(typeof(CartProfile));

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

var _ = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<CartProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/cart/{cartId:int}", async (int cartId, ICartService cartService) =>
// {
//     var cartDto = await cartService.GetCartAsync(cartId);
//     if (cartService == null)
//     {
//         return Results.NotFound();
//     }
//     return Results.Ok(cartDto);
// });

// app.MapPost("/cart", async ([FromBody] CartDto cartDto, ICartService cart) =>
// {
//     var (rowsAffected, cartId) = await cart.SaveCartAsync(cartDto);
//     return rowsAffected > 0
//         ? Results.Ok(DataSavedResponse.CreateSuccess("Cart saved successfully.", rowsAffected, cartId))
//         : Results.BadRequest(DataSavedResponse.CreateFailure("Failed to save cart."));
// });

// app.MapPut("/cart/{cartId:int}/order", async ([FromRoute] int cartId,
//     ICartService cartService, ICartMessenger messenger) =>
// {
//     var cart = await cartService.GetCartAsync(cartId);
//     await messenger.SendCartToSalesOrderAsync(cart);
//     return Results.Ok();
// });
app.UseAuthorization();
app.MapControllers();
app.Run();