using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<CartItem> AddCartItemToCartAsync(CartItem item);
        Task ClearCartAsync(string userId);
        Task<CartItem> RemoveCartItemFromCartAsync(CartItem item);
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task<CartItem> UpdateCartItemAsync(CartItem item);

    }
}
