﻿using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Utilities;

namespace HyperPlayServer.Services;

public class ImageService
{
    private const string IMAGES_FOLDER = "images";

    private readonly UnitOfWork _unitOfWork;

    public ImageService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ImageGame> InsertAsync(ImageRequestDto image, int gameId)
    {
        Game game = await _unitOfWork.GameRepository.GetByIdAsync(gameId, false, true);

        if (game == null)
        {
            throw new InvalidOperationException($"El juego con ID {gameId} no existe.");
        }

        string relativePath = $"{IMAGES_FOLDER}/{Guid.NewGuid()}_{image.File.FileName}";

        ImageGame newImage = new ImageGame
        {
            AltText = image.AltText,
            ImageUrl = relativePath,
            GameId = gameId,
            Game = game
        };

        game.ImageGames.Add(newImage);

        await _unitOfWork.ImageGameRepository.InsertAsync(newImage);

        if (await _unitOfWork.SaveAsync())
        {
            await StoreImageAsync(relativePath, image.File);
            Console.WriteLine($"Imagen guardada en: {relativePath}");
        }

        return newImage;
    }

    public async Task<List<ImageGame>> InsertsAsync(List<IFormFile> image, int gameId)
    {
        List<ImageGame> imageGames = new List<ImageGame>();
        Game game = await _unitOfWork.GameRepository.GetByIdAsync(gameId, false, false);

        foreach (IFormFile imageItem in image) {

           ImageGame img = await InsertAsync(new ImageRequestDto() { File = imageItem, AltText = game.Title}, gameId);

            if (img != null)
            {
                imageGames.Add(img);
            }
        }

        return imageGames;
    }

    public async Task<ImageGame> UpdateFormFileAsync(IFormFile image, string alt, int id)
    {
        ImageGame entity = await _unitOfWork.ImageGameRepository.GetByIdAsync(id);
        entity.AltText = alt;

        _unitOfWork.ImageGameRepository.Update(entity);

        if (await _unitOfWork.SaveAsync() && alt != null)
        {
            await StoreImageAsync(entity.ImageUrl, image);
        }

        return entity;
    }

    public async Task<ImageGame> UpdateAsync(ImageRequestDto image, int id)
    {
        ImageGame entity = await _unitOfWork.ImageGameRepository.GetByIdAsync(id);
        entity.AltText = image.AltText;

        _unitOfWork.ImageGameRepository.Update(entity);

        if (await _unitOfWork.SaveAsync() && entity != null)
        {
            await StoreImageAsync(entity.ImageUrl, image.File);
        }

        return entity;
    }

    private async Task StoreImageAsync(string relativePath, IFormFile file)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file), "El archivo no puede ser nulo.");
        }

        using Stream stream = file.OpenReadStream();

        await FileHelper.SaveAsync(stream, relativePath);
    }

    public async Task<List<ImageGame>> GetImagesByGameIdAsync(int gameId)
    {
        return await _unitOfWork.ImageGameRepository.GetImagesByGameIdAsync(gameId);
    }

    public async Task DeleteAsync(int imageId)
    {
        ImageGame image = await _unitOfWork.ImageGameRepository.GetByIdAsync(imageId);

        if (image == null)
        {
            throw new ArgumentException($"No se encontró ninguna imagen con ID {imageId}");
        }

        try
        {
            await FileHelper.DeleteAsync(image.ImageUrl);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"El archivo {image.ImageUrl} no se encontró en el sistema.");
        }

        _unitOfWork.ImageGameRepository.Delete(image);

        await _unitOfWork.SaveAsync();
    }

}
