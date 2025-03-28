using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;

namespace WebShop.API.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebShopDbContext dbContext;

        public ProductRepository(WebShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await dbContext.Product.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteProductAsync(Guid productId)
        {
            var productDomain = await dbContext.Product.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (productDomain == null) return null;
            dbContext.Product.Remove(productDomain);
            await dbContext.SaveChangesAsync();
            return productDomain;
        }

        public async Task<List<Product>> GetAllProductsAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var products =  dbContext.Product.Include(p => p.Category).AsQueryable();

            //Filtering
            if(!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                }
                if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Price) : products.OrderByDescending(x => x.Price);
                }
            }

            //pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await products.Skip(skipResult).Take(pageSize).ToListAsync();

            //return await dbContext.Product.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await dbContext.Product.Include(p => p.Category).FirstOrDefaultAsync(x=>x.ProductId==productId);
        }

        public async Task<Product?> UpdateProductAsync(Guid productId, Product product)
        {
            var productDomain = await dbContext.Product.FirstOrDefaultAsync(p => p.ProductId==productId);
            if (productDomain == null) return null;
            productDomain.Name = product.Name;
            productDomain.Price = product.Price;
            productDomain.Description = product.Description;
            productDomain.Stock=product.Stock;
            productDomain.ImageUrl = product.ImageUrl;
            productDomain.CategoryId = product.CategoryId;

            await dbContext.SaveChangesAsync();
            return productDomain;

        }
    }
}
