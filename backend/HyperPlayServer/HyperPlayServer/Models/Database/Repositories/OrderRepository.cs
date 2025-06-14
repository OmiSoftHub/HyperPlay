﻿using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities;

namespace HyperPlayServer.Models.Database.Repositories;

public class OrderRepository : Repository<Order, int>
{
    private readonly MyDbContext _context;
    public OrderRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await Context.Set<Order>()
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(og => og.Game)
                    .ThenInclude(g => g.ImageGames)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> GetOrderByUserIdAndOrderIdAsync(int userId, int orderId)
    {
        return await Context.Set<Order>()
            .Include(o => o.User) 
            .Include(o => o.OrderDetails) 
                .ThenInclude(og => og.Game) 
                    .ThenInclude(g => g.ImageGames) 
            .Where(o => o.UserId == userId && o.Id == orderId) 
            .FirstOrDefaultAsync();
    }

    public async Task<List<Order>> GetOrdersByUserId(int userId)
    {
        return await Context.Set<Order>()
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(og => og.Game)
                    .ThenInclude(g => g.ImageGames)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Id) 
            .ToListAsync();
    }


    public async Task<Order> GetRecentOrderByUserId(int userId)
    {
        return await Context.Set<Order>()
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(og => og.Game)
                    .ThenInclude(g => g.ImageGames)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.BillingDate)
            .FirstOrDefaultAsync();
    }
}