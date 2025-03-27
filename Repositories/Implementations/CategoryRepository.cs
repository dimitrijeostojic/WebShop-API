using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;

namespace WebShop.API.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WebShopDbContext dbContext;

        public CategoryRepository(WebShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CategoryExistsByNameAsync(string name)
        {
            return await dbContext.Category.AnyAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await dbContext.Category.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid categoryId)
        {
            var categoryDomain = await dbContext.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
            if (categoryDomain == null) return null;
            dbContext.Category.Remove(categoryDomain);
            await dbContext.SaveChangesAsync();
            return categoryDomain;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await dbContext.Category.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await dbContext.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public async Task<Category?> UpdateCategoryAsync(Guid categoryId, Category category)
        {
            var categoryDomain = await dbContext.Category.FirstOrDefaultAsync(p => p.CategoryId == categoryId);
            if (categoryDomain == null) return null;
            categoryDomain.CategoryName = category.CategoryName;
            await dbContext.SaveChangesAsync();
            return categoryDomain;
        }
    }
}
