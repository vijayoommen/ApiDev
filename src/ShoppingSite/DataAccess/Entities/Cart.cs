using System;
using System.Collections.Generic;

namespace ShoppingSite.DataAccess;

public partial class Cart
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public decimal TotalAmount { get; set; }
    public int? OrderId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
