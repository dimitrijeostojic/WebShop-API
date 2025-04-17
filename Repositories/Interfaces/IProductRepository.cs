using Microsoft.AspNetCore.Mvc;
using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product?> DeleteProductAsync(Guid productId);
        Task<Product?> UpdateProductAsync(Guid productId, Product product);
        Task<List<Product?>> GetMyProductsAsync(string userId);

    }
}
