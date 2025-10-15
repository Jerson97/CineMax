using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Series.Commands.Create;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;

namespace Cinemax.Persistence.Repositories
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SeriesRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<(ServiceStatus, DataCollection<SeriesDto>?, string)> GetSeries(SeriesQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.Search?.Trim().ToLower();

                var page = Math.Max(1, request.Page);
                var pageSize = Math.Max(5, request.Amount);

                var skip = (page - 1) * pageSize;

                var query = _context.Series
                    .Include(x => x.SeriesCategories)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.SeriesDirectors)
                    .ThenInclude(x => x.Director)
                    .Include(x => x.SeriesActors)
                    .ThenInclude(x => x.Actor)
                    .AsNoTracking()
                    .Where(string.IsNullOrEmpty(search) ? x => true : x => x.Title!.ToLower().Contains(search));

                var total = await query.CountAsync(cancellationToken);

                var series = await query
                    .OrderBy(x => x.Title)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var seriesDto = _mapper.Map<List<SeriesDto>>(series);

                var movieDataCollection = new DataCollection<SeriesDto>
                {
                    Items = seriesDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, movieDataCollection, "Serie obtenida exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertSeries(SeriesCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var series = _mapper.Map<Series>(request);

                var validCategoryIds = await _context.Categories
                    .Where(c => request.CategoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync(cancellationToken);

                if (validCategoryIds.Count != request.CategoryIds.Count)
                {
                    return (ServiceStatus.BadRequest, 0, "Una o más categorías no existen.");
                }

                var validDirectorIds = await _context.Directors
                    .Where(d => request.DirectorIds.Contains(d.Id))
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                if (validDirectorIds.Count != request.DirectorIds.Count)
                {
                    return (ServiceStatus.BadRequest, 0, "Una o más directores no existen.");
                }

                var validActorIds = await _context.Actors
                    .Where(a => request.ActorIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken);

                if (validActorIds.Count != request.ActorIds.Count)
                {
                    return (ServiceStatus.BadRequest, 0, "Una o más actores no existen.");
                }

                series.SeriesCategories = validCategoryIds
                    .Select(catId => new SeriesCategory { CategoryId = catId })
                    .ToList();

                series.SeriesDirectors = validDirectorIds
                    .Select(dirId => new SeriesDirector { DirectorId = dirId })
                    .ToList();

                series.SeriesActors = validActorIds
                    .Select(actId => new SeriesActor { ActorId = actId })
                    .ToList();

                await _context.Series.AddAsync(series, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, series.Id, "Serie creada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, 0, $"Error al crear película: {ex.Message}");
            }
            

        }
    }
}
