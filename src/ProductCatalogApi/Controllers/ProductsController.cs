using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productsService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productsService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
    {
        var products = await _productsService.GetProductsByCategoryIdAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchProductsByName(string name)
    {
        var products = await _productsService.SearchProductsByNameAsync(name);
        return Ok(products);
    }
}
