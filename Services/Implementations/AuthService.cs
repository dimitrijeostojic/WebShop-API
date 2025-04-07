using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private string regularUser = "RegularUser";

        public AuthService(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<object> Login(LoginRequestDto loginRequestDto)
        {
            //pronalazenje korisnika po email adresi
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);
            if (user != null)
            {
                //provera lozinke
                var checkPasswordReuslt = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordReuslt)
                {
                    //uzimamo rolu za ovog usera
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //kreiramo token
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                        return new
                        {
                            JwtToken = jwtToken,
                        };
                    }
                }
            }
            return null;
        }

        public async Task<object> Register(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await userManager.FindByNameAsync(registerRequestDto.Username);
            if (existingUser != null)
            {
                return null;
            }

            //kreiranje identity user korisnika
            var identityUser = new ApplicationUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName
            };
            //kreiranje korisnika u sistemu
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            //dodavanje uloga korisniku ako je registracija uspesna
            if (identityResult.Succeeded)
            {

                identityResult = await userManager.AddToRoleAsync(identityUser, regularUser);

                if (identityResult.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(registerRequestDto.Username);
                    var roles = await userManager.GetRolesAsync(user);
                    var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

                    return new
                    {
                        JwtToken = jwtToken
                    };
                }

            }

            return null;
        }
    }
}
