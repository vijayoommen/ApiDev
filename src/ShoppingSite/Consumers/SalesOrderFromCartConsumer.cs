using MassTransit;
using MySiteContracts.Sales;

namespace ShoppingSite.Consumers;

public class SalesOrderFromCartConsumer : IConsumer<SalesOrderIdForCartId>
{
    private ICartService _cartService;

    public SalesOrderFromCartConsumer(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<SalesOrderIdForCartId> context)
    {
        var cartToSalesOrder = context.Message;
        var orderId = await _cartService.SaveOrderIdToCartAsync(cartToSalesOrder.CartId, cartToSalesOrder.OrderId);
    }
}