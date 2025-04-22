using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Repositories.Interfaces;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UserService> logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            logger.LogInformation("[GetAllUsersAsync] Fetching all users");
            var users = await userRepository.GetAllUsersAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                logger.LogInformation("[GetUserRolesAsync] Fetching user roles : {User}", user);
                var roles = await userRepository.GetUserRolesAsync(user);
                var dto = mapper.Map<UserDto>(user);
                dto.Role = roles.FirstOrDefault() ?? "N/A"; // ako korisnik ima više rola, uzimamo prvu
                userDtos.Add(dto);
            }

            return userDtos;

        }

        public async Task<IdentityResult?> DeleteUserAsync(Guid id)
        {
            logger.LogInformation("[GetUserByIdAsync] Getting user with ID: {id}", id);
            var user = await userRepository.GetUserByIdAsync(id.ToString());
            if (user == null)
                return null;
            logger.LogInformation("[DeleteUserAsync] Deleting user : {user}", user);
            return await userRepository.DeleteUserAsync(user);
        }

        public async Task<bool> PromoteUserToManagerAsync(Guid id)
        {
            logger.LogInformation("[SetUserRoleAsync] Attempting to set manager role for user with ID: {id}", id);
            return await userRepository.SetUserRoleAsync(id, "Manager");
        }
    }
}
