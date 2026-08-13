using Microsoft.AspNetCore.Mvc;
using AvatarNavigator.API.Models;
using AvatarNavigator.API.Services;

namespace AvatarNavigator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Order>>> SearchOrders([FromQuery] string term)
        {
            var orders = await _orderService.SearchOrdersAsync(term);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest();

            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
                return NotFound();

            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
