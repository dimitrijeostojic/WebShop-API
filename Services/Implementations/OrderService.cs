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

            foreach (var item in cart.CartItems)
            {
                if (item.Product.Stock<item.Quantity)
                {
                    throw new InvalidOperationException($"Proizvod '{item.Product.Name}' nema dovoljno na lageru.");
                }
                item.Product.Stock -= item.Quantity;
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

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await orderRepository.GetAllOrdersAsync();
        }

        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            return await orderRepository.GetMyOrdersAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await orderRepository.GetOrderByIdAsync(orderId);
            
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            return await orderRepository.UpdateOrderStatusAsync(orderId, status);
        }
    }
}
