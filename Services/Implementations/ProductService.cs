using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var productDomain = await productRepository.CreateProductAsync(product);
            return productDomain;
        }

        public async Task<Product?> DeleteProductAsync(Guid productId)
        {
            var productDomain = await productRepository.DeleteProductAsync(productId);
            if(productDomain==null) return null;
            return productDomain;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await productRepository.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await productRepository.GetProductByIdAsync(productId); 
        }

        public async Task<Product?> UpdateProductAsync(Guid productId, Product product)
        {
            var productDomain = await productRepository.UpdateProductAsync(productId, product);
            if (productDomain == null) return null;
            return productDomain;
        }
    }
}
