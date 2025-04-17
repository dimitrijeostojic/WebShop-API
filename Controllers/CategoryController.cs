using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using WebShop.API.CustomActionFilter;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CategoryController.CreateCategory, createCategoryRequestDto - {createCategoryRequestDto}");
                var categoryDomain = mapper.Map<Category>(createCategoryRequestDto);

                categoryDomain = await categoryService.CreateCategoryAsync(categoryDomain);

                var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryDto.CategoryId }, categoryDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CategoryController.GetAllCategories");
                var categoryDomain = await categoryService.GetAllCategoriesAsync();
                var categoryDto = mapper.Map<List<CategoryDto>>(categoryDomain);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CategoryController.GetCategoryById, categoryId - {categoryId}");
                var categoryDomain = await categoryService.GetCategoryByIdAsync(categoryId);
                if (categoryDomain == null)
                {
                    return NotFound("Kategorija ne postoji");
                }

                var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CategoryController.DeleteCategory, categoryId - {categoryId}");
                var categoryDomain = await categoryService.DeleteCategoryAsync(categoryId);
                if (categoryDomain == null)
                {
                    return NotFound("Kategorija ne postoji");
                }
                var categoryDto = mapper.Map<CategoryDto>(categoryDomain);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{categoryId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CategoryController.UpdateCategory, categoryId - {categoryId} ; updateCategoryRequestDto - {updateCategoryRequestDto}");
                var categoryDomain = mapper.Map<Category>(updateCategoryRequestDto);
                categoryDomain = await categoryService.UpdateCategoryAsync(categoryId, categoryDomain);

                if (categoryDomain == null)
                {
                    return NotFound("Kategorija ne postoji");
                }
                //map to dto
                var categoryDto = mapper.Map<CategoryDto>(categoryDomain);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }
}
