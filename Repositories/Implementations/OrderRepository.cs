using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
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

        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            var orders = await dbContext.Order.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).Where(x => x.UserId == userId).ToListAsync();
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
           return await dbContext.Order.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }
    }
}
