using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using WebShop.API.Models.Domain;

namespace WebShop.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending, int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product?> DeleteProductAsync(Guid productId);
        Task<Product?> UpdateProductAsync(Guid productId, Product product);
    }
}
