using Microsoft.Extensions.Configuration;

namespace ProductCatalogApi;

public interface IAppSettings
{
    SqlServerSettings SqlServerSettings { get; set; }
}

public class AppSettings : IAppSettings
{
    public AppSettings(IConfiguration config)
    {
        AllowedHosts= config["AllowedHosts"] ?? string.Empty;
        SqlServerSettings.ConnectionString = config["SqlServerSettings:ConnectionString"] ?? string.Empty;
    }
    
    public string AllowedHosts { get; set; } = string.Empty; 
    public SqlServerSettings SqlServerSettings { get; set; } = new SqlServerSettings();
}

public class SqlServerSettings 
{
    public string ConnectionString { get; set; } = string.Empty;
}
