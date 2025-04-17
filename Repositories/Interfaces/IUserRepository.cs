using Microsoft.AspNetCore.Identity;
using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<bool> SetUserRoleAsync(Guid id, string role);
    }
}
