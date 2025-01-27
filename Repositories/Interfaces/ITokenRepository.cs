using Microsoft.AspNetCore.Identity;

namespace WebShop.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
