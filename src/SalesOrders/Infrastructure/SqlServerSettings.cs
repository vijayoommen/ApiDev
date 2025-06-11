namespace SalesOrders.Infrastructure;

public interface ISqlServerSettings
{
    string ConnectionString { get; set; }
}

public class SqlServerSettings : ISqlServerSettings
{
    public string ConnectionString { get; set; } = "";
}
