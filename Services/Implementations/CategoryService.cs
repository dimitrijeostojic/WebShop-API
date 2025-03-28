using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Implementations;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {

            var existing = await categoryRepository.CategoryExistsByNameAsync(category.CategoryName);

            if (existing)
                throw new InvalidOperationException("Kategorija već postoji.");
            var categoryDomain = await categoryRepository.CreateCategoryAsync(category);
            return categoryDomain;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return null;
            if (category.Products != null && category.Products.Count > 0)
            {
                throw new InvalidOperationException("Ne mozete obrisati kategoriju koja ima proizvode");
            }
            var categoryDomain = await categoryRepository.DeleteCategoryAsync(categoryId);
            if (categoryDomain == null) return null;
            return categoryDomain;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<Category?> UpdateCategoryAsync(Guid categoryId, Category category)
        {
            return await categoryRepository.UpdateCategoryAsync(categoryId, category);

        }
    }
}
