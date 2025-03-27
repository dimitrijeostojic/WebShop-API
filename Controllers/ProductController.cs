using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            var productDomain = mapper.Map<Product>(createProductRequestDto);

            productDomain = await productService.CreateProductAsync(productDomain);

            var productDto = mapper.Map<ProductDto>(productDomain);

            return CreatedAtAction(nameof(GetProductById), new { productId = productDto.ProductId }, productDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var productsDomain = await productService.GetAllProductsAsync();
            var productsDto = mapper.Map<List<ProductDto>>(productsDomain);
            return Ok(productsDto);
        }

        [HttpGet]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var productDomain = await productService.GetProductByIdAsync(productId);
            if (productDomain == null)
            {
                return NotFound();
            }

            var productDto = mapper.Map<ProductDto>(productDomain);

            return Ok(productDto);
        }

        [HttpDelete]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var productDomain = await productService.DeleteProductAsync(productId);
            if (productDomain == null)
            {
                return NotFound();
            }
            var productDto = mapper.Map<ProductDto>(productDomain);
            return Ok(productDto);
        }

        [HttpPut]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto updateProductRequestDto)
        {
            var productDomain = mapper.Map<Product>(updateProductRequestDto);
            productDomain = await productService.UpdateProductAsync(productId, productDomain);

            if (productDomain == null)
            {
                return NotFound();
            }
            //map to dto
            var productDto = mapper.Map<ProductDto>(productDomain);
            return Ok(productDto);
        }
    }
}
