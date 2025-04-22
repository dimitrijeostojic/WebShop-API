using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShop.API.Enums;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly ILogger<CartService> logger;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger)
        {
            this.cartRepository = cartRepository;
            this.logger = logger;
        }

        public async Task<Product> AddProductToCartAsync(string userId, Guid productId, int quantity)
        {
            logger.LogInformation("[AddProductToCart] UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
            var cart = await GetOrCreateCartAsync(userId);

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                logger.LogInformation("Increased quantity for existing product in cart.");
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                cartItem = await cartRepository.AddCartItemToCartAsync(cartItem);
                logger.LogInformation("Added new product to cart.");
            }

            var item = await cartRepository.GetCartItemAsync(cart.CartId, productId);
            return item?.Product!;
        }

        public async Task ClearCartAsync(string userId)
        {
            logger.LogInformation("[ClearCart] UserId: {UserId}", userId);
            await cartRepository.ClearCartAsync(userId);
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            logger.LogInformation("[GetOrCreateCart] UserId: {UserId}", userId);
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart()
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId
                };
                cart = await cartRepository.CreateCartAsync(cart);
                logger.LogInformation("Created new cart for user.");
            }
            return cart;
        }

        public async Task<Product> RemoveProductFromCartAsync(string userId, Guid productId)
        {
            logger.LogInformation("[RemoveProduct] UserId: {UserId}, ProductId: {ProductId}", userId, productId);
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                logger.LogWarning("Cart not found for user {UserId}", userId);
                throw new Exception("Korpa ne postoji");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                logger.LogWarning("Product not found in cart.");
                return null;
            }

            cartItem = await cartRepository.RemoveCartItemFromCartAsync(cartItem);
            logger.LogInformation("Removed product from cart.");

            return cartItem.Product;
        }

        public async Task<CartItem?> UpdateCartItemQuantityAsync(string userId, Guid productId, int quanity)
        {
            logger.LogInformation("[UpdateQuantity] UserId: {UserId}, ProductId: {ProductId}, NewQuantity: {Quantity}", userId, productId, quanity);
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                logger.LogWarning("Cart not found for user {UserId}", userId);
                throw new Exception("Korpa ne postoji");
            }

            var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem == null)
            {
                logger.LogWarning("Product not found in cart.");
                return null;
            }

            cartItem.Quantity = quanity;
            await cartRepository.UpdateCartItemAsync(cartItem);
            logger.LogInformation("Cart item quantity updated.");

            return cartItem;
        }
    }
}