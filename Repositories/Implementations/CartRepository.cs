using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;

namespace WebShop.API.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly WebShopDbContext dbContext;

        public CartRepository(WebShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CartItem> AddCartItemToCartAsync(CartItem item)
        {
            await dbContext.CartItem.AddAsync(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            await dbContext.Cart.AddAsync(cart);
            await dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await dbContext.Cart.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null && cart.CartItems.Any())
            {
                dbContext.CartItem.RemoveRange(cart.CartItems);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await dbContext.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem> RemoveCartItemFromCartAsync(CartItem item)
        {
            dbContext.CartItem.Remove(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId)
        {
            return await dbContext.CartItem.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem item)
        {
            dbContext.CartItem.Update(item);
            await dbContext.SaveChangesAsync();
            return item;
        }

    }
}
