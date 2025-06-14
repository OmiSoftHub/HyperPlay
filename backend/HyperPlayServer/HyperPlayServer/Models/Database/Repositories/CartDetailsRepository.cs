﻿using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class CartDetailsRepository : Repository<CartDetail, int>
{
    private readonly MyDbContext _context;

    public CartDetailsRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    //public async Task<CartDetail> GetByIdCartDetails(int id)
    //{
    //    return await Context.Set<CartDetail>()
    //        .Include(cd => cd.Game)
    //        .Include(cd => cd.Game.ImageGames)
    //        .FirstOrDefaultAsync(cd => cd.Id == id);
    //}

    public async Task<List<CartDetail>> GetByIdCart(int idCart)
    {
        return await Context.Set<CartDetail>()
        .Where(cd => cd.CartId == idCart)
        .Include(cd => cd.Game)
        .Include(cd => cd.Game.ImageGames)
        .ToListAsync();
    }

    public async Task<CartDetail> GetCartByIds(int idCart, int idGame )
    {
        return await Context.Set<CartDetail>()
            .Include(cd => cd.Game)
                .ThenInclude(g => g.ImageGames)
            .FirstOrDefaultAsync(cd => cd.CartId == idCart && cd.GameId == idGame);
    }
}
