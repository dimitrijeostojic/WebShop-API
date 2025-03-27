using WebShop.API.Enums;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
        }

        public async Task<Order?> CompleteOrderAsync(string userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Korpa je prazna ili ne postoji.");
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                UserId = userId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price // Uzimamo trenutnu cenu proizvoda
                }).ToList(),
                
            };

            var createdOrder = await orderRepository.CreateOrderAsync(order);

            // Isprazni korpu
            await cartRepository.ClearCartAsync(userId);

            return createdOrder;
        }

        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            var orders = await orderRepository.GetMyOrdersAsync(userId);
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            return order;
        }
    }
}
