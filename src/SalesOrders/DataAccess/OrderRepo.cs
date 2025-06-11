using Microsoft.EntityFrameworkCore;
using SalesOrders.DataAccess.Entities;

namespace SalesOrders.DataAccess;

public interface IOrderRepo
{
    Task<Order?> GetOrderAsync(int orderId);
    Task<(int rowsAffected, int orderId)> SaveOrder(Order order);
}

public class OrderRepo : IOrderRepo
{
    private readonly ISalesOrdersDbContext _dbContext;

    public OrderRepo(ISalesOrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.Id == orderId)
            .FirstOrDefaultAsync();
    }

    public async Task<(int rowsAffected, int orderId)> SaveOrder(Order order)
    {
        var orderFromDb = await _dbContext.Orders
            .Where(o => o.Id == order.Id)
            .FirstOrDefaultAsync() ?? new Order
            {
                CustomerId = order.CustomerId,
                CreatedOn = DateTime.UtcNow,
                CartId = order.CartId,
                TotalAmount = 0
            };

        // Remove items that are not in the updated list
        foreach (var item in orderFromDb.OrderItems)
        {
            if (!order.OrderItems.Any(ui => ui.Id == item.Id))
            {
                _dbContext.OrderItems.Remove(item);
            }
        }

        // Add new items that are not in the order
        order.OrderItems
            .Where(oi => oi.Id == 0)
            .ToList()
            .ForEach(oi =>
            {
                oi.CreatedOn = DateTime.UtcNow;
                orderFromDb.OrderItems.Add(oi);
            });

        // Update existing items in the order
        order.OrderItems
            .Where(oi => oi.Id != 0)
            .ToList()
            .ForEach(oi =>
            {
                var existingItem = orderFromDb.OrderItems.FirstOrDefault(i => i.Id == oi.Id);
                if (existingItem != null)
                {
                    existingItem.Price = oi.Price;
                    existingItem.Quantity = oi.Quantity;
                    existingItem.UpdatedOn = DateTime.UtcNow;
                }
            });

        // Calculate total amount
        orderFromDb.TotalAmount = orderFromDb.OrderItems.Sum(i => i.Price * i.Quantity);

        if (orderFromDb.Id == 0)
        {
            _dbContext.Orders.Add(orderFromDb);
        }

        var rowsAffected = await _dbContext.SaveChangesAsync();

        return (rowsAffected, orderFromDb.Id);
    }
}
