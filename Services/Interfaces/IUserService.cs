using Microsoft.AspNetCore.Identity;
using WebShop.API.Models.Dto;

namespace WebShop.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();

        Task<IdentityResult?> DeleteUserAsync(Guid id);
        Task<bool> PromoteUserToManagerAsync(Guid id);

    }
}
