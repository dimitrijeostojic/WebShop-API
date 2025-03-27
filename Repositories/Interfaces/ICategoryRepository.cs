using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        Task<Category?> DeleteCategoryAsync(Guid categoryId);
        Task<Category?> UpdateCategoryAsync(Guid categoryId, Category category);
        Task<bool> CategoryExistsByNameAsync(string name);
    }
}
