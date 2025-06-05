using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ApiKey;

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiKeyName { get; set; } = "X-Api-Key";   
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    private IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> _options;
    private readonly ILoggerFactory _logger;
    private readonly UrlEncoder _enoder;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration) : base(options, logger, encoder)
    {
        _options = options;
        _logger = logger;
        _enoder = encoder;
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headerName = _configuration.GetValue<string>("ApiKeyHeaderName");
        var apiKey = _configuration.GetValue<string>("ApiKey");

        if (!Request.Headers.TryGetValue(headerName, out var apiKeyHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        var apiKeyHeader = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        if (apiKeyHeader != apiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }
        var claims = new[] { 
            new Claim(ClaimTypes.Name, "ApiKeyUser"),
            new Claim(ClaimTypes.Role, "AdminUser")
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
