using WebShop.API.Enums;
using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetMyOrdersAsync(string userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
