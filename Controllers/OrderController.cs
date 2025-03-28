using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "RegularUser")]
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

        [Authorize(Roles = "RegularUser")]
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

        [Authorize(Roles = "RegularUser")]
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var orderDomain = await orderService.GetOrderByIdAsync(id);
            if (orderDomain == null)
            {
                return NotFound("Porudzbina nije pronadjena");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (orderDomain.UserId != userId)
                return Forbid();
            var orderDto = mapper.Map<OrderDto>(orderDomain);
            return Ok(orderDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            var ordersDto = mapper.Map<List<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{orderId:Guid}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]Guid orderId, [FromBody] UpdateOrderStatusDto orderStatusDto)
        {
            var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, orderStatusDto.NewStatus);
            if (updatedOrder == null)
            {
                return NotFound("Porudzbina nije pronadjena");
            }
            var updatedOrderDto = mapper.Map<OrderDto>(updatedOrder);
            return Ok(updatedOrderDto);
        }


    }

}
