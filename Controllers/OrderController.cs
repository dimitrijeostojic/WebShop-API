using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.API.Models.Domain;
using WebShop.API.Models.Dto;
using WebShop.API.Services.Implementations;
using WebShop.API.Services.Interfaces;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var orderDomain = await orderService.CompleteOrderAsync(userId);

            if (orderDomain == null)
                return BadRequest("Neuspešno kreiranje porudžbine.");

            var orderDto = mapper.Map<OrderDto>(orderDomain);

            return Ok(new
            {
                message = "Porudžbina uspešno kreirana.",
                orderDto
            });
        }

        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var ordersDomain = await orderService.GetMyOrdersAsync(userId);

            var ordersDto = mapper.Map<List<OrderDto>>(ordersDomain);
            if (!ordersDomain.Any())
            {
                return Ok(new
                {
                    message = "Ne postoji ni jedna porudzbina",
                    orders = ordersDto
                });
            }
            return Ok(ordersDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var orderDomain = await orderService.GetOrderByIdAsync(id);
            if (orderDomain == null)
            {
                return NotFound();
            }
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            return Ok(orderDto);

        }
    }

}
