using MassTransit;
using MySiteContracts.Sales;

namespace SalesOrders.Services;

public interface IOrderMessenger
{
    Task SendCartToSalesOrderMessage(int cartId, int orderId);
}

public class OrderMessenger : IOrderMessenger
{
    private IBus _bus;

    public OrderMessenger(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendCartToSalesOrderMessage(int cartId, int orderId)
    {
        var message = new SalesOrderIdForCartId
        {
            CartId = cartId,
            OrderId = orderId
        };

        // Send the message to the bus
        await _bus.Publish(message);
    }
}