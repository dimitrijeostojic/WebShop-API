using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShop.API.Data;
using WebShop.API.Models.Domain;
using WebShop.API.Repositories.Interfaces;

namespace WebShop.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await userManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<bool> SetUserRoleAsync(Guid id, string role)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return false;

            var currentRoles = await userManager.GetRolesAsync(user);

            // Samo ako je trenutno RegularUser (ili neka specifična pravila)
            if (!currentRoles.Contains("RegularUser"))
                return false;

            // Ukloni sve postojeće role
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            // Osiguraj da rola postoji
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var addResult = await userManager.AddToRoleAsync(user, role);
            return addResult.Succeeded;
        }
    }
}
