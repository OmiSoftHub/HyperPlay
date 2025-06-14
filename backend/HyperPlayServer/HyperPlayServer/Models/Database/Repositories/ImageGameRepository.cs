﻿using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class ImageGameRepository : Repository<ImageGame, int>
{
    private readonly MyDbContext _context;

    public ImageGameRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ImageGame>> GetAllImagesGamesAsync()
    {
        return await _context.ImagesGame.Include(ig => ig.Game).ToListAsync(); 
    }

    public async Task<List<ImageGame>> GetImagesByGameIdAsync(int gameId)
    {
        return await _context.ImagesGame
            .Where(ig => ig.GameId == gameId) 
            .ToListAsync();
    }

}