using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Security.Claims;
using WebShop.API.CustomActionFilter;
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
        private readonly ILogger<ProductController> logger;

        public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"ProductController.CreateProduct, createProductRequestDto - {createProductRequestDto}");
                var productDomain = mapper.Map<Product>(createProductRequestDto);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                productDomain.CreatedBy = userId;

                productDomain = await productService.CreateProductAsync(productDomain);

                var productDto = mapper.Map<ProductDto>(productDomain);

                return CreatedAtAction(nameof(GetProductById), new { productId = productDto.ProductId }, productDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, int pageNumber=1, int pageSize=1000)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"ProductController.GetAllProducts, filterOn - {filterOn} ; filterQuery - {filterQuery} ; sortBy - {sortBy} ; isAscending - {isAscending} ; pageNumber - {pageNumber} ; pageSize - {pageSize}");
                var productsDomain = await productService.GetAllProductsAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
                var productsDto = mapper.Map<List<ProductDto>>(productsDomain);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet("myproducts")]
        public async Task<IActionResult> GetMyProducts()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                logger.Log(LogLevel.Debug, $"ProductController.GetMyProducts, id - {userId}");
                var productsDomain = await productService.GetMyProductsAsync(userId);
                var productsDto = mapper.Map<List<ProductDto>>(productsDomain);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager, RegularUser")]
        [HttpGet]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"ProductController.GetProductById, productId - {productId}");
                var productDomain = await productService.GetProductByIdAsync(productId);
                if (productDomain == null)
                {
                    return NotFound("Proizvod ne postoji");
                }

                var productDto = mapper.Map<ProductDto>(productDomain);

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"ProductController.DeleteProduct, productId - {productId}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut]
        [Route("{productId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto updateProductRequestDto)
        {

            try
            {
                logger.Log(LogLevel.Debug, $"ProductController.UpdateProduct, productId - {productId} ; updateProductRequestDto - {updateProductRequestDto}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }
    }
}
