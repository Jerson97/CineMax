using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Movies.Commands.Update;
using Cinemax.Application.Features.Movies.Queries.GetAll;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Result;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;

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

        public async Task<(ServiceStatus, DataCollection<MovieDto>?, string)> GetMovie(MovieQuery.MovieQueryRequest request, CancellationToken cancellationToken)
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

        public async Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);

                if (request.CategoryIds?.Any() == true)
                {
                    movie.MovieCategories = request.CategoryIds
                        .Select(catId => new MovieCategory 
                        { 
                            CategoryId = catId, 
                            Movie = movie 
                        })
                        .ToList();
                }

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
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                if (movie == null)
                {
                    return (ServiceStatus.NotFound, null, "Película no encontrada");
                }           

                movie.Title = request.Title ?? movie.Title;
                movie.Description = request.Description ?? movie.Description;
                movie.ReleaseDate = request.ReleaseDate != default ? request.ReleaseDate : movie.ReleaseDate;
                movie.Duration = request.Duration > 0 ? request.Duration : movie.Duration;

                if (request.CategoryIds != null && request.CategoryIds.Any())
                {
                    _context.MovieCategories.RemoveRange(movie.MovieCategories);

                    movie.MovieCategories = request.CategoryIds
                        .Select(catId => new MovieCategory
                        {
                            CategoryId = catId,
                            MovieId = movie.Id
                        })
                        .ToList();
                }

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
