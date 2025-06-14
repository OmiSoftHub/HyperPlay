using Microsoft.EntityFrameworkCore;
using TorchSharp.Modules;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class CartRepository : Repository<Cart, int>
{
    private readonly MyDbContext _context;
    public CartRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Cart> GetByIdCart(int id)
    {
        return await Context.Set<Cart>()
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Game)
                    .ThenInclude(g => g.ImageGames)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cart> GetByUserId(int userId)
    {
        return await Context.Set<Cart>()
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Game)
                    .ThenInclude(g => g.ImageGames)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

}
