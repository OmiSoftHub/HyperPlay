using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class OrderDetailRepository : Repository<OrderDetail, int>
{
    private readonly MyDbContext _context;

    public OrderDetailRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    // Para la vista de usuario
    public async Task<List<OrderDetail>> GetOrderById(int idOrder)
    {
        return await Context.Set<OrderDetail>()
        .Where(cd => cd.OrderId == idOrder)
        .Include(cd => cd.Game)
            .ThenInclude(g => g.ImageGames)
        .ToListAsync();
    }
}