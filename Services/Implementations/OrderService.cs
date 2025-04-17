using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrderService> logger;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, ILogger<OrderService> logger)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
            this.logger = logger;
        }

        public async Task<Order?> CompleteOrderAsync(string userId)
        {
            logger.LogInformation("[CompleteOrder] Starting order process for user: {UserId}", userId);

            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                logger.LogWarning("[CompleteOrder] Empty or missing cart for user: {UserId}", userId);
                throw new InvalidOperationException("Korpa je prazna ili ne postoji.");
            }

            foreach (var item in cart.CartItems)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    logger.LogWarning("[CompleteOrder] Not enough stock for product '{ProductName}' (requested: {Requested}, available: {Available})",
                        item.Product.Name, item.Quantity, item.Product.Stock);
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
                    Price = ci.Product.Price
                }).ToList(),
            };

            var createdOrder = await orderRepository.CreateOrderAsync(order);

            logger.LogInformation("[CompleteOrder] Order created successfully with ID: {OrderId}", createdOrder.OrderId);

            await cartRepository.ClearCartAsync(userId);
            logger.LogInformation("[CompleteOrder] Cart cleared for user: {UserId}", userId);

            return createdOrder;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            logger.LogInformation("[GetAllOrders] Fetching all orders");
            return await orderRepository.GetAllOrdersAsync();
        }

        public async Task<List<Order>> GetMyOrdersAsync(string userId)
        {
            logger.LogInformation("[GetMyOrders] Fetching orders for user: {UserId}", userId);
            return await orderRepository.GetMyOrdersAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            logger.LogInformation("[GetOrderById] Fetching order with ID: {OrderId}", orderId);
            return await orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            logger.LogInformation("[UpdateOrderStatus] Updating status of order {OrderId} to {Status}", orderId, status);
            return await orderRepository.UpdateOrderStatusAsync(orderId, status);
        }
    }
}