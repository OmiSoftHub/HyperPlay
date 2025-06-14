﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Services;

namespace HyperPlayServer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ImagesController : ControllerBase
{
    private readonly ImageService _service;

    public ImagesController(ImageService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ImageGame>> InsertAsync(ImageRequestDto createImage, int gameId)
    {
        ImageGame newImage = await _service.InsertAsync(createImage, gameId);

        return Ok(newImage);
    }

    [HttpPost("newImages")]
    public async Task<ActionResult<List<ImageGame>>> InsertsAsync([FromForm] List<IFormFile> images , int gameId)
    {
        if (images == null || images.Count == 0)
        {
            return BadRequest("Debe proporcionar al menos una imagen.");
        }

        try
        {
            List<ImageGame> newImages = await _service.InsertsAsync(images, gameId);
            return Ok(newImages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("updateImage/{id}")]
    public async Task<ActionResult<ImageGame>> UpdateAsync([FromForm] ImageRequestDto updateImage, int imageId)
    {
        ImageGame imageUpdated = await _service.UpdateAsync(updateImage, imageId);

        return Ok(imageUpdated);
    }

    [HttpGet("images-byGame")]
    public async Task<ActionResult<List<ImageGame>>> GetImagesByGameIdAsync(int gameId)
    {
        return Ok(await _service.GetImagesByGameIdAsync(gameId));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteAsync(int imageId)
    {
       await _service.DeleteAsync(imageId);
       return NoContent();
    }

}
