﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Entities.Enuml;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Services;


namespace HyperPlayServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReserveController : BaseController
{
    private readonly ReserveService _reserveService;
    private readonly StripeService _stripeService;
    private readonly UnitOfWork _unitOfWork;
    
    public ReserveController(ReserveService reserveService, StripeService stripeService, UnitOfWork unitOfWork)
    {
        _reserveService = reserveService;
        _stripeService = stripeService;
        _unitOfWork = unitOfWork;
    }


    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<int>> CreateReserve([FromBody] List<CartDto> cart, [FromQuery] PayMode modeOfPay)
    {
        try
        {
            int userId = GetUserId();

            int reserveId = await _reserveService.CreateReserve(userId, cart, modeOfPay);
            return Ok(reserveId);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado", detail = ex.Message });
        }
    }


    [HttpGet("details")]
    [Authorize]
    public async Task<ActionResult<List<OrderDetailDto>>> GetReserveDetails([FromQuery] int reserveId)
    {
        try
        {
            int userId = GetUserId();

            List<OrderDetailDto> orderDetailDto = await _reserveService.GetReserveDetails(reserveId);
            return Ok(orderDetailDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado", detail = ex.Message });
        }
    }

    [HttpPost("confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmReserve([FromBody] int reserveId)
    {
        try
        {
            int userId = GetUserId();

            int orderId = await _reserveService.ConfirmReserve(reserveId);
            return Ok(new
            {
                message = "Reserva confirmada exitosamente.",
                orderId
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado", detail = ex.Message });
        }
    }

    [HttpDelete("cancel")]
    [Authorize]
    public async Task<IActionResult> CancelReserve()
    {
        try
        {
            int userId = GetUserId();

            await _reserveService.CancelReserve(userId);
            return Ok(new { message = "Reserva cancelada exitosamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado", detail = ex.Message });
        }
    }

    [HttpPost("embedded-checkout")]
    [Authorize]
    public async Task<IActionResult> EmbeddedCheckout([FromBody] int reserveId)
    {
        try
        {
            int userId = GetUserId();

            SessionCreateOptions options = await _stripeService.EmbededCheckout(userId, reserveId);

            SessionService sessionService = new SessionService();
            Session session = await sessionService.CreateAsync(options);

            await _stripeService.SetSessionIdReserve(session.Id, reserveId);

            return Ok(new { clientSecret = session.ClientSecret });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al generar la sesión de pago.", detail = ex.Message });
        }
    }

    [HttpGet("status/{reserveId}")]
    public async Task<ActionResult> SessionStatus(int reserveId)
    {
        Reserve reserve = await _unitOfWork.ReserveRepository.GetReserveById(reserveId);

        Session session = await _stripeService.SessionStatus(reserve.SessionId);

        string paymentStatus = await _stripeService.GetPaymentStatus(reserve.SessionId);

        return Ok(new { status = session.Status, customerEmail = session.CustomerEmail, paymentStatus });
    }


    [HttpGet("total")]
    [Authorize]
    public async Task<ActionResult> GetTotalByReserveId([FromQuery] int reserveId)
    {
        try
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado." });
            }

            int userId = int.Parse(userIdClaim.Value);

            decimal total = await _reserveService.CalculateTotalByReserveId(reserveId);

            return Ok(new { ReserveId = reserveId, Total = total });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error inesperado", detail = ex.Message });
        }
    }

}

