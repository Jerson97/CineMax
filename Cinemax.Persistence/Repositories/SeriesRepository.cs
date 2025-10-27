using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Commands.Delete.SeriesDelete;
using static Cinemax.Application.Features.Series.Commands.Update.SeriesUpdate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;
using static Cinemax.Application.Features.Series.Queries.GetById.SerieDetailQuery;
using static Cinemax.Application.Features.Series.Queries.SeriesByCategory.SerieByCategoryQuery;

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

        public async Task<(ServiceStatus, int?, string)> DeleteSeries(SeriesDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serie = await _context.Series.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (serie == null)
                {
                    return (ServiceStatus.NotFound, null, "Serie no encontrada");
                }

                if (!serie.IsActive)
                {
                    return (ServiceStatus.FailedValidation, serie.Id, "La Serie ya estaba inactiva");
                }

                serie.IsActive = false;

                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, serie.Id, "Serie eliminada exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null, $"Error al eliminar Serie: {ex.Message}");
            }

        }

        public async Task<(ServiceStatus, DataCollection<SeriesDto>?, string)> GetSerieByCategory(SerieByCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var validCategory = await _context.Categories
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == request.Id, cancellationToken);

                if (!validCategory)
                {
                    return (ServiceStatus.NotFound, null, "Categoria no existe");
                }

                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount;

                var skip = (page - 1) * pageSize;

                var query = _context.Series
                    .Include(mc => mc.SeriesCategories)
                    .ThenInclude(c => c.Category)
                    .Include(md => md.SeriesDirectors)
                    .ThenInclude(d => d.Director)
                    .Include(ma => ma.SeriesActor)
                    .ThenInclude(a => a.Actor)
                    .AsNoTracking()
                    .Where(m => m.IsActive == true && m.SeriesCategories.Any(c => c.CategoryId == request.Id));

                var total = await query.CountAsync(cancellationToken);

                var series = await query
                    .OrderBy(x => x.Title)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var serieDto = _mapper.Map<List<SeriesDto>>(series);

                var serieDataCollection = new DataCollection<SeriesDto>
                {
                    Items = serieDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, serieDataCollection, "Serie obtenida exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, SerieByIdDto?, string)> GetSerieId(SerieDetailQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serie = await _context.Series
                    .Include(x => x.Seasons)
                    .Include(x => x.SeriesCategories).ThenInclude(x => x.Category)
                    .Include(x => x.SeriesDirectors).ThenInclude(x => x.Director)
                    .Include(x => x.SeriesActor).ThenInclude(x => x.Actor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (serie == null)
                {
                    return (ServiceStatus.NotFound, null, "Serie no encontrada");
                }

                var serieDto = _mapper.Map<SerieByIdDto>(serie);

                return (ServiceStatus.Ok, serieDto, "Serie obtenida exitosamente");

            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, DataCollection<SeriesDto>?, string)> GetSeries(SeriesQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.Search?.Trim().ToLower();

                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount;

                var skip = (page - 1) * pageSize;

                var query = _context.Series
                    .Include(x => x.SeriesCategories)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.SeriesDirectors)
                    .ThenInclude(x => x.Director)
                    .Include(x => x.SeriesActor)
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

                var seriesDataCollection = new DataCollection<SeriesDto>
                {
                    Items = seriesDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, seriesDataCollection, "Serie obtenida exitosamente");
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

                series.SeriesActor = validActorIds
                    .Select(actId => new SeriesActor { ActorId = actId })
                    .ToList();

                await _context.Series.AddAsync(series, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, series.Id, "Serie creada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, 0, $"Error al crear serie: {ex.Message}");
            }
            

        }

        public async Task<(ServiceStatus, int?, string)> UpdateSeries(SeriesUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var serie = await _context.Series
                    .Include(m => m.SeriesCategories)
                    .Include(m => m.SeriesActor)
                    .Include(m => m.SeriesDirectors)
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);


                if (serie == null)
                {
                    return (ServiceStatus.NotFound, null, "Serie no encontrada");
                }

                serie.Title = request.Title ?? serie.Title;
                serie.Description = request.Description ?? serie.Description;
                serie.ReleaseDate = request.ReleaseDate != default ? request.ReleaseDate : serie.ReleaseDate;

                

                if (request.CategoryIds.Count() > 0)
                {
                    var validCategoryIds = await _context.Categories
                    .Where(c => request.CategoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync(cancellationToken);

                    if (validCategoryIds.Count() > 0)
                    {
                        _context.SeriesCategories.RemoveRange(serie.SeriesCategories);
                        serie.SeriesCategories.Clear();
                        serie.SeriesCategories = validCategoryIds
                        .Select(catId => new SeriesCategory { CategoryId = catId, SeriesId = serie.Id })
                        .ToList();
                    }
            
                }

                if (request.DirectorIds.Count() > 0)
                {
                    var validDirectorIds = await _context.Directors
                    .Where(d => request.DirectorIds.Contains(d.Id))
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                    if (validDirectorIds.Count() > 0)
                    {
                        _context.SeriesDirectors.RemoveRange(serie.SeriesDirectors);
                        serie.SeriesDirectors.Clear();

                        serie.SeriesDirectors = validDirectorIds
                        .Select(dirId => new SeriesDirector { DirectorId = dirId, SeriesId = serie.Id })
                        .ToList();
                    }
                }


                if (request.ActorIds.Count() > 0)
                {
                    var validActorIds = await _context.Actors
                    .Where(a => request.ActorIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken);


                    if (validActorIds.Count() > 0)
                    {
                        _context.SeriesActor.RemoveRange(serie.SeriesActor);
                        serie.SeriesActor.Clear();

                        serie.SeriesActor = validActorIds
                        .Select(actId => new SeriesActor { ActorId = actId, SeriesId = serie.Id })
                        .ToList();
                    }
                }     

                // Actualizar
           
                await _context.SaveChangesAsync(cancellationToken);


                return (ServiceStatus.Ok, serie.Id, "Serie actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al actualizar serie: {ex.Message}");
            }
        }
    }
}
