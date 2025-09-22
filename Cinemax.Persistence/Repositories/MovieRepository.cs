using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Result;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;

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

        public async Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);

                if (request.CategoryIds?.Any() == true)
                {
                    movie.MovieCategories = request.CategoryIds
                        .Select(catId => new MovieCategory { CategoryId = catId, Movie = movie })
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



    }
}
