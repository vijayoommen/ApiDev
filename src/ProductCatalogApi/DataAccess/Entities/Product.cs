namespace ProductCatalogApi.DataAccess.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public virtual ProductCategory? Category { get; set; }
}
