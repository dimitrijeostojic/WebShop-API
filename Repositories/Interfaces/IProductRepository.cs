using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product?> DeleteProductAsync(Guid productId);
        Task<Product?> UpdateProductAsync(Guid productId, Product product);

    }
}
