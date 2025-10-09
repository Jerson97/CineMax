﻿using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Category.Queries.GetById.MovieDetailQuery;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Delete.MovieDelete;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;
using static Cinemax.Application.Features.Movies.Queries.GetAll.MovieQuery;

namespace Cinemax.Persistence.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MovieRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ServiceStatus, int?, string)> DeleteMovie(MovieDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (movie == null)
                {
                    return (ServiceStatus.NotFound, null, "Película no encontrada");
                }

                if (!movie.IsActive)
                {
                    return (ServiceStatus.FailedValidation, movie.Id, "La película ya estaba inactiva");
                }

                movie.IsActive = false;

                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, movie.Id, "Película se elimino");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null, $"\"Error al eliminar película: {ex.Message}");
            }
            

        }

        public async Task<(ServiceStatus, DataCollection<MovieDto>?, string)> GetMovie(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.Search?.Trim().ToLower();

                var page = Math.Max(1, request.Page);
                var pageSize = Math.Max(1, request.Amount);

                var skip = (page - 1) * pageSize;

                //var total = await _context.Movies.CountAsync(cancellationToken);

                var query = _context.Movies
                    .Include(x => x.MovieCategories)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.MovieDirectors)
                    .ThenInclude(x => x.Director)
                    .Include(x => x.MovieActor)
                    .ThenInclude(x => x.Actor)
                    .AsNoTracking()
                    .Where(string.IsNullOrEmpty(search) ? x => true : x => x.Title!.ToLower().Contains(search));

                var total = await query.CountAsync(cancellationToken);

                var movies = await query
                    .OrderBy(x => x.Title)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                 var movieDto = _mapper.Map<List<MovieDto>>(movies);

                var movieDataCollection = new DataCollection<MovieDto>
                {
                    Items = movieDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, movieDataCollection, "Película obtenida exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, MovieDto?, string)> GetMovieId(MovieDetailQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _context.Movies
                    .Include(x => x.MovieCategories)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.MovieDirectors)
                    .ThenInclude(x => x.Director)
                    .Include(x => x.MovieActor)
                    .ThenInclude(x => x.Actor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (movie == null)
                {
                    return (ServiceStatus.NotFound, null, "Película no encontrada");
                }

                var movieDto = _mapper.Map<MovieDto>(movie);

                return (ServiceStatus.Ok, movieDto, "Película obtenida exitosamente");

            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);

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

                    movie.MovieCategories = validCategoryIds
                        .Select(catId => new MovieCategory { CategoryId = catId })
                        .ToList();

                    movie.MovieDirectors = validDirectorIds
                        .Select(dirId => new MovieDirector { DirectorId = dirId })
                        .ToList();

                    movie.MovieActor = validActorIds
                        .Select(actId => new MovieActor { ActorId = actId })
                        .ToList();

                await _context.Movies.AddAsync(movie, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, movie.Id, "Película creada exitosamente");    
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, 0, $"Error al crear película: {ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> UpdateMovie(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _context.Movies
                    .Include(m => m.MovieCategories)
                    .Include(m => m.MovieActor)
                    .Include(m => m.MovieDirectors)  
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);


                if (movie == null)
                {
                    return (ServiceStatus.NotFound, null, "Película no encontrada");
                }           

                movie.Title = request.Title ?? movie.Title;
                movie.Description = request.Description ?? movie.Description;
                movie.ReleaseDate = request.ReleaseDate != default ? request.ReleaseDate : movie.ReleaseDate;
                movie.Duration = request.Duration > 0 ? request.Duration : movie.Duration;

                var validCategoryIds = await _context.Categories
                    .Where(c => request.CategoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync(cancellationToken);


                if (validCategoryIds.Count != request.CategoryIds.Count)
                {
                    return (ServiceStatus.BadRequest, null, "Una o más categorías no existen.");
                }

                var validDirectorIds = await _context.Directors
                    .Where(d => request.DirectorIds.Contains(d.Id))
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                if (validDirectorIds.Count != request.DirectorIds.Count)
                {
                    return (ServiceStatus.BadRequest, null, "Una o más directores no existen.");
                }

                var validActorIds = await _context.Actors
                    .Where(a => request.ActorIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken);

                if (validActorIds.Count != request.ActorIds.Count)
                {
                    return (ServiceStatus.BadRequest, null, "Una o más actores no existen.");
                }

                // Actualizar
                _context.MovieCategories.RemoveRange(movie.MovieCategories);
                _context.MovieDirectors.RemoveRange(movie.MovieDirectors);
                _context.MovieActor.RemoveRange(movie.MovieActor);

                await _context.SaveChangesAsync(cancellationToken);

                movie.MovieCategories.Clear();
                movie.MovieDirectors.Clear();
                movie.MovieActor.Clear();


                movie.MovieCategories = validCategoryIds
                    .Select(catId => new MovieCategory { CategoryId = catId, MovieId = movie.Id })
                    .ToList();

                movie.MovieDirectors = validDirectorIds
                    .Select(dirId => new MovieDirector { DirectorId = dirId, MovieId = movie.Id })
                    .ToList();

                movie.MovieActor = validActorIds
                    .Select(actId => new MovieActor { ActorId = actId, MovieId = movie.Id })
                    .ToList();

                await _context.SaveChangesAsync(cancellationToken);


                return (ServiceStatus.Ok, movie.Id, "Película actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al actualizar película: {ex.Message}");
            }
        }

    }
}
