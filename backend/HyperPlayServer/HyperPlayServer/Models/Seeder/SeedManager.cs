using HyperPlayServer.Models.Database;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Seeders;
using HyperPlayServer.Services;

namespace HyperPlayServer.Models.Seeder;

public class SeedManager
{
    private readonly MyDbContext _context;
    private readonly IAService _iaService;
    private readonly DetailsViewService _detailsService;

    public SeedManager(MyDbContext context, IAService iaService, DetailsViewService detailsService)
    {
        _context = context;
        _iaService = iaService;
        _detailsService = detailsService;
    }

    public void SeedAll()
    {
        var gameRequirementsSeeder = new GameRequirementsSeeder(_context);
        gameRequirementsSeeder.Seed();

        var gameSeeder = new GameSeeder(_context);
        gameSeeder.Seed();

        var userSeeder = new UserSeeder(_context);
        userSeeder.Seed();
        
        var reviewSeeder = new ReviewSeeder(_context, _iaService, _detailsService);
        reviewSeeder.Seed();
    }
}
