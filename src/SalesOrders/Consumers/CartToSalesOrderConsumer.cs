using AutoMapper;
using MassTransit;
using MySiteContracts.Sales;
using SalesOrders.Models.Responses;

namespace SalesOrders;

public class CartToSalesOrderConsumer : IConsumer<MySiteContracts.Sales.CartToSalesOrder>
{
    private readonly IMapper _mapper;
    private readonly IOrderServices _orderService;

    public CartToSalesOrderConsumer(IMapper mapper, IOrderServices orderServices)
    {
        _mapper = mapper;
        _orderService = orderServices;
    }

    public async Task Consume(ConsumeContext<CartToSalesOrder> context)
    {
        var cart = context.Message;
        var orderDto = _mapper.Map<OrderDto>(cart);

        // Save the order and its items
        var (rowsAffected, orderId) = await _orderService.SaveOrderAsync(orderDto);

        // Optionally, you can return or log the saved order ID
        Console.WriteLine($"Order saved with ID: {orderId}, Rows affected: {rowsAffected}");
    }

}
