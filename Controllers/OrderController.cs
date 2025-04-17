using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using WebShop.API.CustomActionFilter;
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
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, IMapper mapper, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Authorize(Roles = "RegularUser")]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder()
        {
            try
            {
                logger.Log(LogLevel.Debug, $"OrderController.PlaceOrder");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "RegularUser")]
        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                logger.Log(LogLevel.Debug, $"OrderController.GetMyOrders");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "RegularUser")]
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"OrderController.GetOrderById, id - {id}");
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                logger.Log(LogLevel.Debug, $"OrderController.GetAllOrders");
                var orders = await orderService.GetAllOrdersAsync();
                var ordersDto = mapper.Map<List<OrderDto>>(orders);
                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{orderId:Guid}/status")]
        [ValidateModel]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]Guid orderId, [FromBody] UpdateOrderStatusDto orderStatusDto)
        {
            try
            {
                logger.Log(LogLevel.Debug, $"OrderController.UpdateOrderStatus, orderId - {orderId} ; orderStatusDto - {orderStatusDto}");
                var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, orderStatusDto.NewStatus);
                if (updatedOrder == null)
                {
                    return NotFound("Porudzbina nije pronadjena");
                }
                var updatedOrderDto = mapper.Map<OrderDto>(updatedOrder);
                return Ok(updatedOrderDto);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                throw;
            }
        }


    }

}
