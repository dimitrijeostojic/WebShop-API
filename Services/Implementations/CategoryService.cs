using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Implementations;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<CategoryService> logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            logger.LogInformation("[CreateCategory] Attempting to create category: {CategoryName}", category.CategoryName);

            var existing = await categoryRepository.CategoryExistsByNameAsync(category.CategoryName);

            if (existing)
            {
                logger.LogWarning("[CreateCategory] Category with name '{CategoryName}' already exists", category.CategoryName);
                throw new InvalidOperationException("Kategorija već postoji.");
            }

            var categoryDomain = await categoryRepository.CreateCategoryAsync(category);
            logger.LogInformation("[CreateCategory] Successfully created category with ID: {CategoryId}", categoryDomain.CategoryId);

            return categoryDomain;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid categoryId)
        {
            logger.LogInformation("[DeleteCategory] Attempting to delete category with ID: {CategoryId}", categoryId);

            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                logger.LogWarning("[DeleteCategory] Category with ID {CategoryId} not found", categoryId);
                return null;
            }

            if (category.Products != null && category.Products.Count > 0)
            {
                logger.LogWarning("[DeleteCategory] Cannot delete category with ID {CategoryId} because it has products", categoryId);
                throw new InvalidOperationException("Ne mozete obrisati kategoriju koja ima proizvode");
            }

            var categoryDomain = await categoryRepository.DeleteCategoryAsync(categoryId);
            if (categoryDomain == null)
            {
                logger.LogWarning("[DeleteCategory] Failed to delete category with ID: {CategoryId}", categoryId);
                return null;
            }

            logger.LogInformation("[DeleteCategory] Successfully deleted category with ID: {CategoryId}", categoryId);
            return categoryDomain;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            logger.LogInformation("[GetAllCategories] Fetching all categories");
            return await categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            logger.LogInformation("[GetCategoryById] Fetching category with ID: {CategoryId}", categoryId);
            return await categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<Category?> UpdateCategoryAsync(Guid categoryId, Category category)
        {
            logger.LogInformation("[UpdateCategory] Updating category with ID: {CategoryId}", categoryId);
            return await categoryRepository.UpdateCategoryAsync(categoryId, category);
        }
    }
}
