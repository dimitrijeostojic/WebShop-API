using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebShop.API.Models.Domain;

namespace WebShop.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(ApplicationUser user, List<string> roles);
    }
}
