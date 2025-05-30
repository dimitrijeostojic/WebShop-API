﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebShop.API.CustomActionFilter;
using WebShop.API.Models.Dto;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        // konstruktor koji prima userManager za upravljanje korisnicima
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        //Metoda za registraciju korisnika
        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {   
            var response = await authService.Register(registerRequestDto);
            if (response!=null)
            {
                return Ok(response);
            }
            return BadRequest("This user already exist");
        }

        //metoda za logovanje
        [HttpPost]
        [Route("Login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = await authService.Login(loginRequestDto);
            if (response != null)
            {
                return Ok(response);
            }
            return Unauthorized();
        }

    }
}
