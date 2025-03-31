using Microsoft.EntityFrameworkCore;
using WebShop.API.Enums;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IOrderRepository orderRepository;

        public CartService(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<Product> AddProductToCartAsync(string userId, Guid productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
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
            }

            var item = await cartRepository.GetCartItemAsync(cart.CartId, productId);

            return item?.Product!;
        }

        public async Task ClearCartAsync(string userId)
        {
            await cartRepository.ClearCartAsync(userId);
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart()
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                cart = await cartRepository.CreateCartAsync(cart);
            }

            return cart;
        }

        public async Task<Product> RemoveProductFromCartAsync(string userId, Guid productId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Korpa ne postoji");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null) return null;

            var product = cartItem.Product;

            cartItem = await cartRepository.RemoveCartItemFromCartAsync(cartItem);

            return product;
        }

        public async Task<CartItem?> UpdateCartItemQuantityAsync(string userId, Guid productId, int quanity)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Korpa ne postoji");

            var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem == null) return null;

            cartItem.Quantity = quanity;
            await cartRepository.UpdateCartItemAsync(cartItem);
            return cartItem;
        }
    }
}
