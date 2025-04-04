namespace ProductCatalogApi.DataAccess.Entities;

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; } 
    public DateTime? UpdatedOn { get; set; }
}
