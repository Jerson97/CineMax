using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Favorites.Command.Create.FavoriteCreate;

namespace Cinemax.Persistence.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FavoriteRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ServiceStatus, DataCollection<FavoriteDto>?, string)>GetAllFavorites(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var favorites = await _context.Favorites
                    .Include(f => f.Movie)
                    .Include(f => f.Series)
                    .Where(f => f.UserId == userId)
                    .ToListAsync(cancellationToken);

                if (!favorites.Any())
                    return (ServiceStatus.NotFound, null, "No se encontraron favoritos.");

                var result = favorites.Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    Title = f.Movie != null ? f.Movie.Title : f.Series!.Title,
                    Type = f.Movie != null ? "movie" : "series",
                    ImageUrl = f.Movie != null ? f.Movie.ImageUrl : f.Series!.ImageUrl
                }).ToList();

                var dataCollection = new DataCollection<FavoriteDto>
                {
                    Total = result.Count,
                    Page = 1,
                    Pages = 1,
                    Items = result
                };

                return (ServiceStatus.Ok, dataCollection, "Lista de favoritos obtenida correctamente.");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error interno: {ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertFavorite(FavoriteCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Validar que el usuario exista
                var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
                if (!userExists)
                {
                    return (ServiceStatus.NotFound, null, "El usuario no existe.");
                }

                // Validar tipo de contenido
                if (string.IsNullOrWhiteSpace(request.Type))
                {
                    return (ServiceStatus.BadRequest, null, "Debes indicar el tipo de contenido.");
                }

                var type = request.Type.ToLower();
                if (type != "movie" && type != "series")
                {
                    return (ServiceStatus.BadRequest, null, "Tipo de contenido inválido. Usa 'movie' o 'series'.");
                }
                    

                // Validar que el contenido exista
                bool contentExists = type == "movie"
                    ? await _context.Movies.AnyAsync(m => m.Id == request.Id, cancellationToken)
                    : await _context.Series.AnyAsync(s => s.Id == request.Id, cancellationToken);

                if (!contentExists)
                {
                    return (ServiceStatus.NotFound, null, "El contenido no existe.");
                }

                // Verificar si ya esta en favoritos
                bool alreadyExists = await _context.Favorites.AnyAsync(f =>
                    f.UserId == request.UserId &&
                    ((type == "movie" && f.MovieId == request.Id) ||
                     (type == "series" && f.SeriesId == request.Id)),
                     cancellationToken);

                if (alreadyExists)
                {
                    return (ServiceStatus.BadRequest, null, "Este contenido ya está en tus favoritos.");
                }

                // Crear nuevo registro
                var favorite = new Favorite
                {
                    UserId = request.UserId,
                    MovieId = type == "movie" ? request.Id : null,
                    SeriesId = type == "series" ? request.Id : null
                };

                await _context.Favorites.AddAsync(favorite, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Created, favorite.Id, "Agregado a favoritos correctamente.");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error interno: {ex.Message}");
            }
        }
    }
}
