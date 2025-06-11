using AutoMapper;
using MassTransit;
using MySiteContracts.Sales;

namespace ShoppingSite.Services;

public interface ICartMessenger
{
    Task SendCartToSalesOrderAsync(CartDto cartDto);
}

public class CartMessenger : ICartMessenger
{
    private IBus _bus;
    private IMapper _mapper;

    public CartMessenger(IBus bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }

    public async Task SendCartToSalesOrderAsync(CartDto cartDto)
    {
        var cartToSalesOrder = _mapper.Map<CartDto, CartToSalesOrder>(cartDto);
        var message = new CartToSalesOrder(cartDto.CartId, cartDto.CustomerId, cartToSalesOrder.CartItems);
        await _bus.Publish(message);
    }
}