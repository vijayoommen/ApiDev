namespace ProductCatalogApi.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public static class JwtAuthentication
{
    private const string Key = "this_is_a_very_secret_key_and_it_has_to_be_longer_than_32_characters"; // Replace with your secret key
    public static void SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7014";
                options.Audience = "https://localhost:7014";
                options.RequireHttpsMetadata = true; // Set to true in production
                options.ClaimsIssuer = "https://localhost:7014";
                options.IncludeErrorDetails = true;
                options.MapInboundClaims = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "https://localhost:7014",
                    ValidAudience = "https://localhost:7014", 
                    // IssuerSigningKeyValidator = (securityKey, securityToken, validationParameters) =>
                    // {
                    //     // Custom validation logic can be added here if needed
                    //     return true;
                    // }
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Log the exception or handle it as needed
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // Custom logic after token validation can be added here if needed
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        // Custom logic for forbidden requests can be added here if needed
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization(configure =>
        {
            configure.AddPolicy("AdminUsersPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });

            configure.AddPolicy("RegularUserPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "User", "Admin");
            });
        });
    }

    public static string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)); // Replace with your secret key

        var roles = new[] { "User", "Admin" }; // Example roles, replace with actual roles as needed
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, username),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, username),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "vijayoommen"),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            // new Claim("roles", System.Text.Json.JsonSerializer.Serialize(roles), System.IdentityModel.Tokens.Jwt.JsonClaimValueTypes.JsonArray) // Adding array as a claim
        });
        // var roles = new [] { "User", "Admin" }; // Example roles, replace with actual roles as needed
        // var jsonRoles = System.Text.Json.JsonSerializer.Serialize(roles);
        // claims.AddClaim(new Claim(ClaimTypes.Role, jsonRoles, System.IdentityModel.Tokens.Jwt.JsonClaimValueTypes.JsonArray));

        var token = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(10),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddMinutes(-1),
            Claims = claims.Claims.ToDictionary(c => c.Type, c => (object)c.Value),
            Issuer = "https://localhost:7014",
            Audience = "https://localhost:7014",
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(token));
    }

}