using AutoFixture;
using ProductCatalogApi.Services;
using ProductCatalogApi.DataAccess.Entities;
using ProductCatalogApi.DataAccess.Repos;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace ProductCatalogApiTests;

public class ProductsServiceTests
{
    [Fact]
    public async Task Should_GetProductFromId()
    {
        // Arrange
        var fixture = new Fixture();
        var productList = fixture.Build<Product>()
            .CreateMany(5)
            .ToList();
        
        var productCatalogRepoMock = new Mock<IProductCatalogRepo>();
        productCatalogRepoMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => productList.FirstOrDefault(p => p.Id == id));

        var productService = new ProductsService(productCatalogRepoMock.Object);
        var expectedProduct = productList.Last();

        // act
        var actualProduct = await productService.GetProductByIdAsync(expectedProduct.Id);

        // assert
        Assert.Equal(expectedProduct, actualProduct);
    }
}