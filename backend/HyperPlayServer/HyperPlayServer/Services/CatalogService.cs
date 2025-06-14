using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Dtos;
using HyperPlayServer.Models.Database.Repositories;
using HyperPlayServer.Models.Mappers;
using Microsoft.EntityFrameworkCore;
using HyperPlayServer.Models.Database.Entities.Enum;

namespace HyperPlayServer.Services
{
    public class CatalogService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly GameCardMapper _gameCardMapper;
        private SmartSearchService smartSearchService;

        public CatalogService(UnitOfWork unitOfWork, GameCardMapper gameCardMapper)
        {
            _unitOfWork = unitOfWork;
            _gameCardMapper = gameCardMapper;
            smartSearchService = new SmartSearchService();
        }

        public async Task<CatalogDto> FilterAndSortGamesAsync(GameFilterDto filter)
        {
            IQueryable<Game> query = _unitOfWork.GameRepository.GetIncludingImages();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                List<string> gameTitles = await _unitOfWork.GameRepository.GetAllTitles();
                // Usar SmartSearchService para realizar la búsqueda inteligente ???????? MODIFICAR
                var matchedTitles = smartSearchService.Search(filter.SearchText, gameTitles);

                // Si hay coincidencias, aplicamos un filtro sobre el título de los juegos ???????? MODIFICAR
                if (matchedTitles != null && matchedTitles.Any())
                {
                    query = query.Where(g => matchedTitles.Contains(g.Title));
                }
                else {
                    return new CatalogDto();
                }
            }

            int drmValue = (int)filter.DrmFree;
            if (drmValue == -1)
            { }
            else if (Enum.IsDefined(typeof(Drm), drmValue))
            {
                Drm drmEnum = (Drm)drmValue;
                query = query.Where(g => g.DrmFree == drmEnum);
            }

            int genreValue = (int)filter.Genre;
            if (genreValue == -1)
            { }
            else if (Enum.IsDefined(typeof(Genre), genreValue))
            {
                Genre genreEnum = (Genre)genreValue;
                query = query.Where(g => g.Genre == genreEnum);
            }


            // Asumiendo que filter.SortCriteria es un entero
            int sortCriteriaValue = (int)filter.SortCriteria; // Por ejemplo, 0 para HighestPrice
            if (Enum.IsDefined(typeof(SortCriteria), sortCriteriaValue))
            {
                SortCriteria sortCriteriaEnum = (SortCriteria)sortCriteriaValue;

                switch (sortCriteriaEnum)
                {
                    case SortCriteria.HighestPrice:
                        query = query.OrderByDescending(g => g.Price);
                        break;
                    case SortCriteria.LowestPrice:
                        query = query.OrderBy(g => g.Price);
                        break;
                    case SortCriteria.ZToA:
                        query = query.OrderByDescending(g => g.Title);
                        break;
                    default:
                        query = query.OrderBy(g => g.Title);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(g => g.Title); // Orden por defecto
            }

            int page = filter.Page > 0 ? filter.Page : 1;
            int resultsPerPage = filter.ResultsPerPage > 0 ? filter.ResultsPerPage : 10;

            // Calcular el total de juegos antes de la paginación
            int totalGames = await query.CountAsync();

            // Calcular el número total de páginas
            int totalPages = (int)Math.Ceiling(totalGames / (double)resultsPerPage);


            List<Game> games = await query
                .Skip((page - 1) * resultsPerPage)
                .Take(resultsPerPage)
                .ToListAsync();

            // Mapear la lista de juegos a GameCardDto
            GameCardMapper gameCardMapper = new GameCardMapper();
            List<GameCardDto> cards = gameCardMapper.ListToDto(games).ToList();

            CatalogDto catalogoDto = new CatalogDto
            {
                Games = cards,
                TotalPages = totalPages
            };

            return catalogoDto;

        }

        public async Task<List<GameCardDto>> GetNewGamesRelease()
        {

            List<Game> games = await _unitOfWork.GameRepository.GetNewGamesRelease();

            return _gameCardMapper.ListToDto(games).ToList();
        }

        public async Task<List<GameCardDto>> GetSaleGames()
        {
            int[] ids = { 1, 30, 29, 7, 8 };
            List<Game> games = new List<Game>();

            foreach (int id in ids)
            {
                Game game = await _unitOfWork.GameRepository.GetByIdAsync(id, false, true);
                if (game != null)
                {
                    games.Add(game);
                }
            }

            return _gameCardMapper.ListToDto(games).ToList();
        }

    }
}
