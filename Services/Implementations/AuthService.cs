using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthService(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<object> Login(LoginRequestDto loginRequestDto)
        {
            //pronalazenje korisnika po email adresi
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
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
            //kreiranje identity user korisnika
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            //kreiranje korisnika u sistemu
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            //dodavanje uloga korisniku ako je registracija uspesna
            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        var user = await userManager.FindByEmailAsync(registerRequestDto.Username);
                        var roles = registerRequestDto.Roles;
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                       
                        return new
                        {
                            JwtToken = jwtToken
                        };
                    }
                }
            }

            return null;
        }
    }
}
