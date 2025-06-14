﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Services;

namespace HyperPlayServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetOrderByUserAndOrderIdAsync(int orderId)
        {
            try
            {
                int userId = GetUserId();

                OrderDto orderDto = await _orderService.GetOrderByUserIdAndOrderIdAsync(userId, orderId);
                return Ok(orderDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", detail = ex.Message });
            }
        }

        [HttpGet("all-user-orders")]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserId()
        {
            try
            {
                int userId = GetUserId();

                List<OrderDto> orders = await _orderService.GetOrdersByUserIdAsync(userId);

                return Ok(orders);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado.", detail = ex.Message });
            }
        }
    }
}

