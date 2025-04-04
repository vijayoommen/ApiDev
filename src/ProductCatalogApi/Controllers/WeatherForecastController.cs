using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.DataAccess;
using ProductCatalogApi.DataAccess.Entities;

namespace ProductCatalogApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAppSettings _appSettings;
    private readonly ProductCatalogDbContext _dbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IAppSettings appSettings, ProductCatalogDbContext dbContext)
    {
        _logger = logger;
        _appSettings = appSettings;
        _dbContext = dbContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var results = await _dbContext.ProductCategories.ToListAsync();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
