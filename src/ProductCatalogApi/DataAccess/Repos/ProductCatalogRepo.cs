using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.DataAccess;
using ProductCatalogApi.DataAccess.Entities;

namespace ProductCatalogApi;

public interface IProductCatalogRepo
{
    Task<List<Product>> FindProducts(Func<Product, bool> predicate);
    Task<List<Product>> FindProductsByNameAsync(string productName);
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
}

public class ProductCatalogRepo : IProductCatalogRepo
{
    private IProductCatalogDbContext _dbContext;

    public ProductCatalogRepo(IProductCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.Include(_ => _.Category).ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task<List<Product>> FindProductsByNameAsync(string productName)
    {
        return await _dbContext.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{productName}%"))
            .ToListAsync();
    }

    public async Task<List<Product>> FindProducts(Func<Product, bool> predicate)
    {
        return await Task.FromResult(_dbContext.Products.Where(predicate).ToList());
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _dbContext.Products
            .Include(_ => _.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }
}
