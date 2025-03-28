using WebShop.API.Enums;
using WebShop.API.Models.Domain;

namespace WebShop.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CompleteOrderAsync(string userId);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetMyOrdersAsync(string userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
