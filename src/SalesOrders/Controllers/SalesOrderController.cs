using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesOrders;
using SalesOrders.Models.Responses;

namespace MyApp.Namespace
{
    [Route("api/salesorders")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private IOrderServices _orderServices;

        public SalesOrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderAsync(int orderId)
        {
            var order = await _orderServices.GetOrderAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrderAsync([FromBody] OrderDto orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null)
            {
                return BadRequest("Invalid order data.");
            }

            var (rowsAffected, orderId) = await _orderServices.SaveOrderAsync(orderDto);
            
            return CreatedAtAction(nameof(GetOrderAsync), new { orderId }, null);
        }
    }
}
