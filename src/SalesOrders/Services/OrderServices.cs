using AutoMapper;
using SalesOrders.DataAccess;
using SalesOrders.Models.Responses;
using SalesOrders.Services;

namespace SalesOrders;

public interface IOrderServices
{
    Task<OrderDto> GetOrderAsync(int orderId);
    Task<(int rowsAffected, int orderId)> SaveOrderAsync(OrderDto orderDto);
}

public class OrderServices : IOrderServices
{
    private IMapper _mapper;
    private IOrderRepo _orderRepo;
    private IOrderMessenger _orderMessenger;

    public OrderServices(IMapper mapper, IOrderRepo orderRepo, IOrderMessenger orderMessenger)
    {
        _mapper = mapper;
        _orderRepo = orderRepo;
        _orderMessenger = orderMessenger;
    }

    public async Task<OrderDto> GetOrderAsync(int orderId)
    {
        var order = await _orderRepo.GetOrderAsync(orderId);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<(int rowsAffected, int orderId)> SaveOrderAsync(OrderDto orderDto)
    {
        var orderEntity = _mapper.Map<DataAccess.Entities.Order>(orderDto);
        var (rowsAffected, orderId) = await _orderRepo.SaveOrder(orderEntity);
        if (rowsAffected > 0)
        {
            // Send a message to the bus to notify that the order has been saved
            await _orderMessenger.SendCartToSalesOrderMessage(orderDto.CartId.GetValueOrDefault(), orderId);
        }
        return (rowsAffected, orderEntity.Id);
    }

}
