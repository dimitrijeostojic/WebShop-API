using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            this.userService = userService;
            this.logger = logger;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                logger.Log(LogLevel.Debug, "CartController.GetMyCart");
                var users = await userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid id)
        {
            try
            {
                logger.Log(LogLevel.Debug, "CartController.GetMyCart");
                var result = await userService.DeleteUserAsync(id);
                if (!result.Succeeded)
                {
                    return NotFound("Korisnik nije pronađen");
                }
                return Ok("Korisnik uspešno obrisan.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

        }



        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> PromoteUserToManager([FromRoute] Guid id)
        {
            try
            {
                logger.Log(LogLevel.Debug, "CartController.GetMyCart");
                var success = await userService.PromoteUserToManagerAsync(id);
                if (!success)
                    return NotFound("Korisnik nije pronađen ili nije regularan korisnik");

                return Ok("Korisnik uspešno promovisan u menadžera.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

        }
    }
}
