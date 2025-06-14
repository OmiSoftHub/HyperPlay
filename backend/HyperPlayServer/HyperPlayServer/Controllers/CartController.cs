﻿using Microsoft.AspNetCore.Mvc;
using HyperPlayServer.Services;
using HyperPlayServer.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace HyperPlayServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : BaseController
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<ActionResult<List<CartDto>>> UpdateCart([FromBody] List<CartDto> cartResponseDtos)
    {
        try
        {
            int userId = GetUserId();

            int cartId = userId;

            return await _cartService.UpdateCart(cartResponseDtos, cartId);
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

    [HttpGet("cartById")]
    [Authorize]
    public async Task<ActionResult<List<CartDto>>> GetCartById()
    {
        try
        {
            int userId = GetUserId();

            int cartId = userId;

            return await _cartService.GetCartById(cartId);
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


    [HttpGet("cartByGames")]
    public async Task<ActionResult<List<CartGameDto>>> GetCartGames([FromQuery] List<int> gameIds)
    {
        return await _cartService.GetCartGames(gameIds);
    }


    [HttpPut("mergeCart")]
    [Authorize]
    public async Task<ActionResult<List<CartDto>>> MergeCart([FromBody] List<CartDto> cartResponseDtos)
    {
        try
        {
            int userId = GetUserId();

            int cartId = userId;

            return await _cartService.MergeCart(cartResponseDtos, cartId);
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

    [HttpDelete("deleteCartDetail")]
    [Authorize]
    public async Task<IActionResult> DeleteCartDetail(int gameId)
    {
        try
        {

            int userId = GetUserId();

            int cartId = userId; 

            await _cartService.DeleteCartDetailAsync(gameId, cartId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocurrió un error al eliminar el producto.", Error = ex.Message });
        }
    }

}


