using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.API.Models.Dto;

namespace WebShop.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> Register(RegisterRequestDto registerRequestDto);
        Task<object> Login(LoginRequestDto loginRequestDto);

    }
}
