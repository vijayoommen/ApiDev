using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ShoppingSite.DataAccess;
using ShoppingSite.Responses;

namespace ShoppingSite;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int cartId);
    Task<(int rowsAffected, int cartId)> SaveCartAsync(CartDto cartDto);
    Task<int> SaveOrderIdToCartAsync(int cartId, int orderId);
}

public class CartService : ICartService
{
    private IShoppingSiteRepo _repo;
    private IMapper _mapper;
    private ILogger<CartService> _logger;

    public CartService(
        IShoppingSiteRepo shoppingSiteRepo,
        IMapper mapper,
        ILogger<CartService> logger)
    {
        _repo = shoppingSiteRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartDto> GetCartAsync(int cartId)
    {
        _logger.LogInformation("Fetching cart items for cart ID: {CartId}", cartId);
        var cart = await _repo.GetCartAsync(cartId);

        if (cart == null)
        {
            _logger.LogWarning("Cart with ID {CartId} not found", cartId);
            return new CartDto();
        }

        var cartDto = _mapper.Map<Cart, CartDto>(cart);

        return cartDto;
    }

    public async Task<(int rowsAffected, int cartId)> SaveCartAsync(CartDto cartDto)
    {
        var cartId = cartDto.CartId;
        _logger.LogInformation("Saving cart items for cart ID: {CartId}", cartId);

        var cart = _mapper.Map<CartDto, Cart>(cartDto);

        var (rowsAffected, id) = await _repo.SaveCart(cart);

        _logger.LogInformation("Cart with ID {CartId} saved successfully: {rowsAffected}", id, rowsAffected);

        return (rowsAffected, id);
    }

    public async Task<int> SaveOrderIdToCartAsync(int cartId, int orderId)
    {
        _logger.LogInformation("Saving order ID {OrderId} to cart ID {CartId}", orderId, cartId);

        var rowsAffected = await _repo.SaveOrderIdToCartAsync(cartId, orderId);

        _logger.LogInformation("Order ID {OrderId} saved to cart ID {CartId} with rows affected: {RowsAffected}", orderId, cartId, rowsAffected);

        return rowsAffected;
    }
}
