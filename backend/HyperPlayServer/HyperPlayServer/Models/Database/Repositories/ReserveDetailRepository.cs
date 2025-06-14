﻿using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class ReserveDetailsRepository : Repository<ReserveDetail, int>
{
    private readonly MyDbContext _context;

    public ReserveDetailsRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ReserveDetail>> GetReserveById(int idReserve)
    {
        return await Context.Set<ReserveDetail>()
        .Where(rd => rd.ReserveId == idReserve)
        .Include(rd => rd.Game)
            .ThenInclude(g => g.ImageGames) 
        .ToListAsync();
    }

    public async Task<ReserveDetail> GetReserveByIds(int idReserve, int idGame)
    {
        return await Context.Set<ReserveDetail>()
            .Include(cd => cd.Game)
                .ThenInclude(g => g.ImageGames)
            .FirstOrDefaultAsync(cd => cd.ReserveId == idReserve && cd.GameId == idGame);
    }

}