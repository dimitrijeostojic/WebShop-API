using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            
            var categoryDomain = mapper.Map<Category>(createCategoryRequestDto);

            categoryDomain = await categoryService.CreateCategoryAsync(categoryDomain);

            var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryDto.CategoryId }, categoryDto);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categoryDomain = await categoryService.GetAllCategoriesAsync();
            var categoryDto = mapper.Map<List<CategoryDto>>(categoryDomain);
            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            var categoryDomain = await categoryService.GetCategoryByIdAsync(categoryId);
            if (categoryDomain == null)
            {
                return NotFound("Kategorija ne postoji");
            }

            var categoryDto = mapper.Map<CategoryDto>(categoryDomain);

            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            var categoryDomain = await categoryService.DeleteCategoryAsync(categoryId);
            if (categoryDomain == null)
            {
                return NotFound("Kategorija ne postoji");
            }
            var categoryDto = mapper.Map<CategoryDto>(categoryDomain);
            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
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
    }
}
