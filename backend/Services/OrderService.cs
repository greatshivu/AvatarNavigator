using AvatarNavigator.API.Models;
using AvatarNavigator.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AvatarNavigator.API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> SearchOrdersAsync(string searchTerm);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string searchTerm)
        {
            return await _context.Orders
                .Where(o => o.OrderNumber.Contains(searchTerm) || o.CustomerName.Contains(searchTerm))
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
