using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;

namespace WebShop.API.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(string userId);
        Task<Product> AddProductToCartAsync(string userId, Guid productId, int quantity);
        Task<Product> RemoveProductFromCartAsync(string userId, Guid productId);
        Task ClearCartAsync(string userId);
        Task<CartItem?> UpdateCartItemQuantityAsync(string userId, Guid productId, int quanity);

    }
}
