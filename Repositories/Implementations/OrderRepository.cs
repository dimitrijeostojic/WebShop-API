using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
using WebShop.API.Enums;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;

namespace WebShop.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebShopDbContext dbContext;

        public OrderRepository(WebShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await dbContext.Order.AddAsync(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await dbContext.Order.Include(o=>o.OrderItems).ThenInclude(oi=>oi.Product).ToListAsync();
        }

        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            return await dbContext.Order.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).Where(x => x.UserId == userId).ToListAsync();
           
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
           return await dbContext.Order.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var orderDomain = await dbContext.Order.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (orderDomain==null)
            {
                return null;
            }
            orderDomain.OrderStatus = status;
            await dbContext.SaveChangesAsync();
            return orderDomain;
        }
    }
}
