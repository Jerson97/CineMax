using System.Net;
using System.Security.Claims;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Features.Favorites.Command.Delete
{
    public class FavoriteDelete
    {
        public class FavoriteDeleteRequest : IRequest<MessageResult<int>>
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string? Type { get; set; }
        }

        public class Manejador : IRequestHandler<FavoriteDeleteRequest, MessageResult<int>>
        {
            private readonly IFavoriteRepository _favoriteRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public Manejador(IFavoriteRepository favoriteRepository, IHttpContextAccessor httpContextAccessor)
            {
                _favoriteRepository = favoriteRepository;
                _httpContextAccessor = httpContextAccessor;
            }
            public async Task<MessageResult<int>> Handle(FavoriteDeleteRequest request, CancellationToken cancellationToken)
            {
                // Obtener el UserId desde el token 
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim))
                    throw new ErrorHandler(HttpStatusCode.Unauthorized, "Usuario no autenticado.");

                request.UserId = int.Parse(userIdClaim);

                var (status, favoriteId, message) = await _favoriteRepository.DeleteFavorite(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, 0);
            }
        }
    }
}
