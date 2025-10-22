using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;
using static Cinemax.Application.Features.Seasons.Queries.EpisodeBySeason.EpisodeBySeasonQuery;

namespace Cinemax.Persistence.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SeasonRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ServiceStatus, DataCollection<EpisodeDto>?, string)> GetEpisodeBySeason(EpisodeBySeasonQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {                
                var season = await _context.Seasons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (season == null)
                {
                    return (ServiceStatus.NotFound, null, "Temporada no encontrada");
                }

                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount;


                var skip = (page - 1) * pageSize;

                var query =  _context.Episodes
                    .AsNoTracking()
                    .Where(e => e.SeasonId == season.Id);


                var total = await query.CountAsync(cancellationToken);

                var episode = await query
                    .OrderBy(e => e.Number)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var episodeDtos = _mapper.Map<List<EpisodeDto>>(episode);

                var episodeDataCollection = new DataCollection<EpisodeDto>
                {
                    Items = episodeDtos,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, episodeDataCollection, "Episodios obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertSeason(SeasonCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {

                if (!_context.Series.Any(s => s.Id == request.SeriesId))
                {
                    return (ServiceStatus.NotFound, null, "La serie no existe");
                }

                if (_context.Seasons.Any(e => e.Number == request.Number && e.SeriesId == request.SeriesId))
                {
                    return (ServiceStatus.BadRequest, null, "La temporada ya existe para esta serie");
                }

                var season = _mapper.Map<Season>(request);

                await _context.Seasons.AddAsync(season, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, season.Id, "Temporada creada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al crear temporada: {ex.Message}");
            }
        }
    }
}
