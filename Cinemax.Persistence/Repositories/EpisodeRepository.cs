using AutoMapper;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Episodes.Command.Create.EpisodeCreate;

namespace Cinemax.Persistence.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EpisodeRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<(ServiceStatus status, int?, string message)> InsertEpisode(EpisodeCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {

                if (!_context.Seasons.Any(s => s.Id == request.SeasonId))
                {
                    return (ServiceStatus.NotFound, null, "La temporada no existe");
                }

                if (_context.Episodes.Any(e => e.Number == request.Number && e.SeasonId == request.SeasonId))
                {
                    return (ServiceStatus.BadRequest, null, "El número de episodio ya existe en esta temporada");
                }


                var episode = _mapper.Map<Episode>(request);

                await _context.Episodes.AddAsync(episode, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, episode.Id, "Episodio creado exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al crear el episodio: {ex.Message}");
            }
        }
    }
}
