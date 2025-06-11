namespace MySiteContracts.Sales;

public class CartToSalesOrder
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public List<CartItemForSalesOrderDto> CartItems { get; set; } = new List<CartItemForSalesOrderDto>();

    public CartToSalesOrder(int cartId, int customerId, List<CartItemForSalesOrderDto> cartItems)
    {
        CartId = cartId;
        CustomerId = customerId;
        CartItems = cartItems;
    }

    public class CartItemForSalesOrderDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
