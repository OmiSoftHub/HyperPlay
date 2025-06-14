﻿using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Generic;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Dtos;

namespace HyperPlayServer.Models.Mappers;

public class AdminMapper
{
    public AdminUserDto ToAdminUserDto(User user)
    {
        return new AdminUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Rol = user.Rol,
        };
    }

    public List<AdminUserDto> ToListAdminUser(List<User> users)
    {
        List<AdminUserDto> adminUserDtos = new List<AdminUserDto>();

        foreach (User user in users)
        {
            adminUserDtos.Add(ToAdminUserDto(user));
        }

        return adminUserDtos;
    }

    public AdminGameDto ToAdminGameDto(Game game)
    {
        return new AdminGameDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            Stock = game.Stock,
            ImageGame = game.ImageGames.FirstOrDefault(),
        };
    }

    public List<AdminGameDto> ToListAdminGameDto(List<Game> games)
    {
        List<AdminGameDto> adminGameDtos = new List<AdminGameDto>();

        foreach (Game game in games)
        {
            adminGameDtos.Add(ToAdminGameDto(game));
        }

        return adminGameDtos;
    }

    private string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }


    public ImageRequestDto ToImageRequestDto(ImageGame imageGame)
    {
        string filePath = imageGame.ImageUrl;

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(filePath))
        {
            Headers = new HeaderDictionary(),
            ContentType = GetContentType(filePath)
        };

        return new ImageRequestDto()
        {
            AltText = imageGame.AltText,
            File = formFile
        };
    }

    public List<ImageRequestDto> ToListImageRequestDto(List<ImageGame> imageGames)
    {
        List<ImageRequestDto> imageRequestDtos = new List<ImageRequestDto>();

        foreach (ImageGame imageGame in imageGames) {
            imageRequestDtos.Add(ToImageRequestDto(imageGame));
        }

        return imageRequestDtos;
    }


    public AdminFormGameDto ToAdminFormGameDto(Game game)
    {
        List<int> imagesId = new List<int>();

        foreach (ImageGame image in game.ImageGames) { 
            imagesId.Add(image.Id);
        }

        return new AdminFormGameDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            Stock = game.Stock,
            GameRequirementsId = game.GameRequirementsId,
            Description = game.Description,
            Sinopsis = game.Sinopsis,
            Genre = game.Genre,
            DrmFree = game.DrmFree,
            ReleaseDate = game.ReleaseDate,
            ImagesId = imagesId
        };
    }
}
