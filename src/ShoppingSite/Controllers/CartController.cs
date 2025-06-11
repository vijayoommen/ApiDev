using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite;
using ShoppingSite.Responses;
using ShoppingSite.Services;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;
        private ICartMessenger _cartMessenger;

        public CartController(ICartService cartService, ICartMessenger cartMessenger)
        {
            _cartService = cartService;
            _cartMessenger = cartMessenger;
        }

        [HttpGet("{cartId:int}")]
        public async Task<IActionResult> GetCartAsync(int cartId)
        {
            var cartDto = await _cartService.GetCartAsync(cartId);
            if (cartDto == null)
            {
                return NotFound();
            }
            return Ok(cartDto);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCartAsync([FromBody] CartDto cartDto)
        {
            var (rowsAffected, cartId) = await _cartService.SaveCartAsync(cartDto);
            return rowsAffected > 0
                ? Ok(DataSavedResponse.CreateSuccess("Cart saved successfully.", rowsAffected, cartId))
                : BadRequest(DataSavedResponse.CreateFailure("Failed to save cart."));
        }

        [HttpPut("{cartId:int}/order")]
        public async Task<IActionResult> SendCartToSalesOrderAsync([FromRoute] int cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null)
            {
                return NotFound();
            }
            if (cart.OrderId.HasValue)
            {
                return BadRequest("Cart has already been converted to an order.");
            }
            await _cartMessenger.SendCartToSalesOrderAsync(cart);
            return Ok();
        }
    }
}
