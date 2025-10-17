using AutoMapper;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;

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
