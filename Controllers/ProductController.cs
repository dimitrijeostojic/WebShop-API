using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            var productDomain = mapper.Map<Product>(createProductRequestDto);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            productDomain.CreatedBy = userId;

            productDomain = await productService.CreateProductAsync(productDomain);

            var productDto = mapper.Map<ProductDto>(productDomain);

            return CreatedAtAction(nameof(GetProductById), new { productId = productDto.ProductId }, productDto);
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, int pageNumber=1, int pageSize=1000)
        {
            var productsDomain = await productService.GetAllProductsAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            var productsDto = mapper.Map<List<ProductDto>>(productsDomain);
            return Ok(productsDto);
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var productDomain = await productService.GetProductByIdAsync(productId);
            if (productDomain == null)
            {
                return NotFound("Proizvod ne postoji");
            }

            var productDto = mapper.Map<ProductDto>(productDomain);

            return Ok(productDto);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var product = await productService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            if (roles.Contains("Admin") || (roles.Contains("Manager") && product.CreatedBy == userId))
            {
                var deletedProduct = await productService.DeleteProductAsync(productId);
                var productDto = mapper.Map<ProductDto>(deletedProduct);
                return Ok(productDto);
            }
            return Forbid("Nemate dozvolu da obrišete ovaj proizvod.");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto updateProductRequestDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var product = await productService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            if (roles.Contains("Admin") || ((roles.Contains("Manager") && product.CreatedBy == userId)))
            {
                var updatedProductDomain = mapper.Map<Product>(updateProductRequestDto);
                updatedProductDomain.CreatedBy = product.CreatedBy;

                var updatedProduct = await productService.UpdateProductAsync(productId, updatedProductDomain);
                var productDto = mapper.Map<ProductDto>(updatedProduct);
                return Ok(productDto);
            }
            return Forbid("Nemate dozvolu da obrišete ovaj proizvod.");
        }
    }
}
