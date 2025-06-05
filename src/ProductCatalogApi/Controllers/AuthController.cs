using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Infrastructure;

namespace ProductCatalogApi.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody]LoginRequest request)
    {
        if (request.Email == "admin@localhost" || request.Password == "password")
        {
            var token = JwtAuthentication.GenerateJwtToken(request.Email);           
            return Ok(new { token });    
        }
        
        return Unauthorized(new { message = "Invalid username or password" });
    }
}
