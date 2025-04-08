using ProductCatalogApi.DataAccess.Entities;
using ProductCatalogApi.DataAccess.Repos;

namespace ProductCatalogApi.Services;

public interface IProductsService
{
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
    Task<List<Product>> SearchProductsByNameAsync(string name);
}

public class ProductsService : IProductsService
{
    private readonly IProductCatalogRepo _productCatalogRepo;

    public ProductsService(IProductCatalogRepo productCatalogRepo)
    {
        _productCatalogRepo = productCatalogRepo;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _productCatalogRepo.GetProductsAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productCatalogRepo.GetProductByIdAsync(id);
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _productCatalogRepo.GetProductsByCategoryIdAsync(categoryId);
    }

    public async Task<List<Product>> SearchProductsByNameAsync(string name)
    {
        return await _productCatalogRepo.FindProductsByNameAsync(name);
    }
}
