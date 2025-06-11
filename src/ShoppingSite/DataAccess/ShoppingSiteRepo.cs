using Microsoft.EntityFrameworkCore;
using ShoppingSite.DataAccess;

namespace ShoppingSite;

public interface IShoppingSiteRepo
{
    Task<Cart?> GetCartAsync(int cartId);
    Task<(int rowsAffected, int cartId)> SaveCart(Cart cart);

    Task<int> SaveOrderIdToCartAsync(int cartId, int orderId);
}

public class ShoppingSiteRepo : IShoppingSiteRepo
{
    private IShoppingSiteDbContext _dbContext;
    public ShoppingSiteRepo(IShoppingSiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetCartAsync(int cartId)
    {
        return await _dbContext.Carts
            .Include(c => c.CartItems)
            .Where(c => c.Id == cartId)
            .FirstOrDefaultAsync();
    }

    public async Task<(int rowsAffected, int cartId)> SaveCart(Cart cart)
    {
        var cartId = cart.Id;
        var cartFromDb = await _dbContext.Carts
            .Where(c => c.Id == cartId)
            .FirstOrDefaultAsync() ?? new Cart
            {
                CustomerId = cart.CustomerId,
                CreatedOn = DateTime.UtcNow,
                TotalAmount = 0
            };

        // Remove items that are not in the updated list
        foreach (var item in cartFromDb.CartItems)
        {
            if (!cart.CartItems.Any(ui => ui.Id == item.Id))
            {
                _dbContext.CartItems.Remove(item);
            }
        }

        // Add new items that are not in the cart
        cart.CartItems
            .Where(ci => ci.Id == 0)
            .ToList()
            .ForEach(ci =>
            {
                ci.CreatedOn = DateTime.UtcNow;
                cartFromDb.CartItems.Add(ci);
            });
        
        // Update existing items in the cart
        cart.CartItems
            .Where(ci => ci.Id != 0)
            .ToList()
            .ForEach(ci =>
            {
                var itemFromDb = cartFromDb.CartItems.FirstOrDefault(c => c.Id == ci.Id);
                if (itemFromDb == null)
                {
                    ci.CreatedOn = DateTime.UtcNow;
                    cartFromDb.CartItems.Add(ci);
                }
                else
                {
                    itemFromDb.ProductId = ci.ProductId;
                    itemFromDb.Quantity = ci.Quantity;
                    itemFromDb.Price = ci.Price;
                    itemFromDb.UpdatedOn = DateTime.UtcNow;
                }
            });

        cartFromDb.TotalAmount = cartFromDb.CartItems.Sum(ci => ci.Price * ci.Quantity);
        cartFromDb.UpdatedOn = DateTime.UtcNow;
        if (cartFromDb.Id == 0)
        {
            _dbContext.Carts.Add(cartFromDb);
        }
        else
        {
            _dbContext.Carts.Update(cartFromDb);
        }
        var rowsAffected = await _dbContext.SaveChangesAsync();

        cart.Id = cartFromDb.Id; // Update the cart ID in the original object

        return (rowsAffected, cartFromDb.Id);
    }

    public async Task<int> SaveOrderIdToCartAsync(int cartId, int orderId)
    {
        return await _dbContext.Carts
            .Where(c => c.Id == cartId)
            .ExecuteUpdateAsync(c => c.SetProperty(c => c.OrderId, orderId));
    }
}
