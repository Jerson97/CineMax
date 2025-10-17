using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.Features.Seasons.Command.Create;
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
                var seriesExists = _context.Series.Any(s => s.Id == request.SeriesId);               

                if (!seriesExists)
                {
                    return (ServiceStatus.NotFound, null, "La serie no existe");
                }

                var seasonExists = _context.Seasons.Any(s => s.Number == request.Number && s.SeriesId == request.SeriesId);
                if (seasonExists) 
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
