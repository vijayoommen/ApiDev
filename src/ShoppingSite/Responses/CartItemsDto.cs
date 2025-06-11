namespace ShoppingSite;

public class CartDto
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public decimal Total { get; set; }
    public int? OrderId { get; set; }
    public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}

public class CartItemDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}