using Microsoft.AspNetCore.Mvc;
using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Models.Mappers;
using HyperPlayServer.Services;

namespace HyperPlayServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _gameService;
        private readonly SmartSearchService _smartSearchService;

        public CatalogController(CatalogService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("catalog-search")]
        public async Task<ActionResult<CatalogDto>> Filter([FromQuery] GameFilterDto filter)
        {
            CatalogDto games = await _gameService.FilterAndSortGamesAsync(filter);
            return Ok(games);
        }


        [HttpGet("new-releases")]
        public async Task<ActionResult<List<GameCardDto>>> GetNewGamesRelease()
        {
            List<GameCardDto> newGames = await _gameService.GetNewGamesRelease();
            return Ok(newGames);
        }

        [HttpGet("sales")]
        public async Task<ActionResult<List<GameCardDto>>> GetSaleGames()
        {
            List<GameCardDto> saleGames = await _gameService.GetSaleGames();
            return Ok(saleGames);
        }
    }
}
