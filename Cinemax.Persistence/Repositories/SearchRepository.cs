using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Search.Queries.SearchByName.SearchByNameQuery;

namespace Cinemax.Persistence.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        public SearchRepository(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }
        public async Task<(ServiceStatus, DataCollection<SearchByNameDto>?, string)> SearchByName(SearchByNameQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.Search?.Trim().ToLower();

                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount;
                var skip = (page - 1) * pageSize;

                //  Búsqueda de películas
                var movieQuery = _context.Movies
                    .AsNoTracking()
                    .Where(x => string.IsNullOrEmpty(search) || x.Title!.ToLower().Contains(search));

                var movies = await movieQuery.ToListAsync(cancellationToken);
                var movieDtos = _mapper.Map<List<SearchByNameDto>>(movies);
                movieDtos.ForEach(x => x.Type = "Movie");

                //  Búsqueda de series
                var seriesQuery = _context.Series
                    .AsNoTracking()
                    .Where(x => string.IsNullOrEmpty(search) || x.Title!.ToLower().Contains(search));

                var series = await seriesQuery.ToListAsync(cancellationToken);
                var seriesDtos = _mapper.Map<List<SearchByNameDto>>(series);
                seriesDtos.ForEach(x => x.Type = "Series");

                //  Combinar resultados
                var allResults = movieDtos.Concat(seriesDtos)
                    .OrderBy(x => x.Title)
                    .ToList();

                if (!allResults.Any())
                {
                    return (ServiceStatus.NoContent, null, "No se encontraron resultados.");
                }

                //  Paginación combinada
                var total = allResults.Count;
                var pagedResults = allResults.Skip(skip).Take(pageSize).ToList();

                var data = new DataCollection<SearchByNameDto>
                {
                    Items = pagedResults,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, data, "Resultados obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }


    }
}
