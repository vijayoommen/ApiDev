namespace SalesOrders.DataAccess.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public int? CartId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
