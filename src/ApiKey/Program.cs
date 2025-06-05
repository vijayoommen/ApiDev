using ApiKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();


builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", 
        options =>
        {
            options.ApiKey = configuration["ApiKey"];
            options.ApiKeyName = configuration["ApiKeyHeaderName"];
        });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyAdminRole", policy =>
    {
        policy.AuthenticationSchemes.Add("ApiKey");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("AdminUser");
    });
    options.AddPolicy("ApiKeyReadOnlyRole", policy =>
    {
        policy.AuthenticationSchemes.Add("ApiKey");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("ReadOnlyUser");
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", [Authorize(AuthenticationSchemes = "ApiKey", Policy = "ApiKeyAdminRole")] (IHttpContextAccessor httpContextAccessor) =>
{
    var user  = httpContextAccessor.HttpContext?.User.Identity;
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return Results.Ok(new {user, forecast});
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
