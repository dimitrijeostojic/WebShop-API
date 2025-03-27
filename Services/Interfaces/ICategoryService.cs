using WebShop.API.Models.Domain;

namespace WebShop.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        Task<Category?> DeleteCategoryAsync(Guid categoryId);
        Task<Category?> UpdateCategoryAsync(Guid categoryId, Category category);
    }
}
