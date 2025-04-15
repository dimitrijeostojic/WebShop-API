using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.API.CustomActionFilter;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Implementations;
using WebShop.API.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;
        private readonly ILogger<CartController> logger;

        public CartController(ICartService cartService, IMapper mapper, ILogger<CartController> logger)
        {
            this.cartService = cartService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Authorize(Roles = "RegularUser")]
        [HttpGet("myCart")]
        public async Task<IActionResult> GetMyCart()
        {
            try
            {
                logger.Log(LogLevel.Debug, "CartController.GetMyCart");
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                var cartDomain = await cartService.GetOrCreateCartAsync(userId);
                var cartDto = mapper.Map<CartDto>(cartDomain);
                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return NotFound();
            }
        }


        [Authorize(Roles = "RegularUser")]
        [HttpPost("addItemToCart")]
        [ValidateModel]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequestDto request)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CartController.AddItemToCart, request - {request}");
                if (request.Quantity <= 0)
                {
                    logger.LogWarning($"Količina mora biti veća od 0.  request - {request}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "RegularUser")]
        [HttpDelete]
        [Route("items/{productId:Guid}")]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid productId)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CartController.RemoveItemFromCart, productId - {productId}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "RegularUser")]
        [HttpDelete("clearCart")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CartController.ClearCart");
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                await cartService.ClearCartAsync(userId);

                return Ok(new { message = "Korpa uspešno ispražnjena." });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "RegularUser")]
        [HttpPut]
        [Route("cartItemQuantity/{productId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCartItemQunatity(Guid productId, UpdateCartItemRequestDto updateCartItemRequest)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"CartController.RemoveItemFromCart, productId - {productId} ; updateCartItemRequest - {updateCartItemRequest}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

        }

    }
}
