using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.DataAccess;
using ProductCatalogApi.DataAccess.Entities;
using ProductCatalogApi.DataAccess.Repos;

namespace ProductCatalogApiTests;

public class ProductCatalogRepoTests
{
    private ProductCatalogRepo _productCatalogRepo;

    public ProductCatalogRepoTests()
    {
        // create context
        var context = SetupDb();
        _productCatalogRepo = new ProductCatalogRepo(context);
    }

    [Fact]
    public async Task ProductCatalogRepoTestsAsync()
    {
        // get a product from the DB
        var products = await _productCatalogRepo.GetProductsAsync();

        // assert
        Assert.NotNull(products);
        Assert.True(products.Count > 0);
        Assert.All(products, p => Assert.NotNull(p.Category));
    }

    [Fact]
    public async Task Should_FindProductsByNameAsync()
    {
        // arrange
        var productName = "Delta";

        // act
        var products = await _productCatalogRepo.FindProductsByNameAsync(productName);

        // assert
        Assert.NotNull(products);
    }

    [Fact]
    public async Task ShouldNot_FindProductsByNameAsync()
    {
        // arrange
        var productName = "This should not exist";

        // act
        var products = await _productCatalogRepo.FindProductsByNameAsync(productName);

        // assert
        Assert.Empty(products);
    }


    private ProductCatalogDbContext SetupDb()
    {
        // setup our DB
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ProductCatalogDbContext(options);
        
        context.Database.EnsureCreated();
        
        // Seed the database with test data if needed
        Fixture fixture = new Fixture();
        var categories = fixture.Build<ProductCategory>()
            .CreateMany(2)
            .ToList();
            
        categories.ForEach(c => context.ProductCategories.Add(c));

        categories.ForEach(c =>
        {
            var products = fixture.Build<Product>()
                .With(p => p.CategoryId, c.Id)
                .With(p => p.Category, c)
                .CreateMany(3)
                .ToList();

            context.Products.AddRange(products);
        });

        // Add a test product with a specific name
        var testProduct = fixture.Build<Product>()
            .With(p => p.Name, "Test Delta Product")
            .With(p => p.Description, "Test Description")
            .With(p => p.ImageUrl, "http://example.com/image.jpg")
            .With(p => p.Price, 9.99m)
            .With(p => p.CategoryId, categories[0].Id)
            .With(p => p.IsActive, true)
            .With(p => p.CreatedOn, DateTime.UtcNow)
            .With(p => p.UpdatedOn, DateTime.UtcNow)
            .Create();
        context.Products.Add(testProduct);

        context.SaveChanges();

        return context;
    }
}
