using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Implementations;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "RegularUser")]
        [HttpGet("myCart")]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var cartDomain = await cartService.GetOrCreateCartAsync(userId);
            var cartDto = mapper.Map<CartDto>(cartDomain);
            return Ok(cartDto);
        }


        [Authorize(Roles = "RegularUser")]
        [HttpPost("addItemToCart")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequestDto request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest("Količina mora biti veća od 0.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var productDomain = await cartService.AddProductToCartAsync(userId, request.ProductId, request.Quantity);
            var productDto = mapper.Map<ProductDto>(productDomain);

            return Ok(new
            {
                message = $"Proizvod '{productDto.Name}' dodat u korpu.",
                product = productDto
            });
        }

        [Authorize(Roles = "RegularUser")]
        [HttpDelete]
        [Route("items/{productId:Guid}")]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var productDomain = await cartService.RemoveProductFromCartAsync(userId, productId);
            if (productDomain == null)
            {
                return NotFound("Proizvod nije pronadjen");
            }

            var productDto = mapper.Map<ProductDto>(productDomain);

            return Ok(new
            {
                message = $"Proizvod '{productDomain.Name}' uklonjen iz korpe.",
                product = productDto
            });
        }

        [Authorize(Roles = "RegularUser")]
        [HttpDelete("clearCart")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await cartService.ClearCartAsync(userId);

            return Ok(new { message = "Korpa uspešno ispražnjena." });
        }

        [Authorize(Roles = "RegularUser")]
        [HttpPut]
        [Route("cartItemQuantity/{productId:Guid}")]
        public async Task<IActionResult> UpdateCartItemQunatity(Guid productId, UpdateCartItemRequestDto updateCartItemRequest)
        {
            if (updateCartItemRequest.Quantity <= 0)
            {
                return BadRequest("Količina mora biti veća od 0.");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var updatedCartItem = await cartService.UpdateCartItemQuantityAsync(userId, productId, updateCartItemRequest.Quantity);

            if (updatedCartItem == null)
            {
                return NotFound("Proizvod nije pronađen u korpi.");
            }
            var updatedCartItemDto = mapper.Map<CartItemDto>(updatedCartItem);
            return Ok(updatedCartItemDto);

        }

    }
}
