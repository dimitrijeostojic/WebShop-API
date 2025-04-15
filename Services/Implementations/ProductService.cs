using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<ProductService> logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            logger.LogInformation("[CreateProduct] Creating product: {ProductName}", product.Name);
            var productDomain = await productRepository.CreateProductAsync(product);
            logger.LogInformation("[CreateProduct] Product created with ID: {ProductId}", productDomain.ProductId);
            return productDomain;
        }

        public async Task<Product?> DeleteProductAsync(Guid productId)
        {
            logger.LogInformation("[DeleteProduct] Attempting to delete product with ID: {ProductId}", productId);
            var productDomain = await productRepository.DeleteProductAsync(productId);
            if (productDomain == null)
            {
                logger.LogWarning("[DeleteProduct] Product with ID {ProductId} not found", productId);
                return null;
            }
            logger.LogInformation("[DeleteProduct] Product with ID {ProductId} deleted", productId);
            return productDomain;
        }

        public async Task<List<Product>> GetAllProductsAsync(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending, int pageNumber, int pageSize)
        {
            logger.LogInformation("[GetAllProducts] Fetching products with filters: filterOn={FilterOn}, filterQuery={FilterQuery}, sortBy={SortBy}, isAscending={IsAscending}, pageNumber={PageNumber}, pageSize={PageSize}",
                filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return await productRepository.GetAllProductsAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
        }

        public async Task<List<Product?>> GetMyProductsAsync(string userId)
        {
            logger.LogInformation("[GetMyProductsAsync] Fetching product with ID: {userId}", userId);
            return await productRepository.GetMyProductsAsync(userId);
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            logger.LogInformation("[GetProductById] Fetching product with ID: {ProductId}", productId);
            return await productRepository.GetProductByIdAsync(productId);
        }

        public async Task<Product?> UpdateProductAsync(Guid productId, Product product)
        {
            logger.LogInformation("[UpdateProduct] Updating product with ID: {ProductId}", productId);
            var productDomain = await productRepository.UpdateProductAsync(productId, product);
            if (productDomain == null)
            {
                logger.LogWarning("[UpdateProduct] Product with ID {ProductId} not found", productId);
                return null;
            }
            logger.LogInformation("[UpdateProduct] Product with ID {ProductId} updated successfully", productId);
            return productDomain;
        }
    }
}
