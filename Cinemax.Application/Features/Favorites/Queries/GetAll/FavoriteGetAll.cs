using System.Net;
using System.Security.Claims;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Features.Favorites.Queries.GetAll
{
    public class FavoriteGetAll
    {
        public class FavoriteGetAllRequest : IRequest<MessageResult<DataCollection<FavoriteDto>>> { }

        public class Manejador : IRequestHandler<FavoriteGetAllRequest, MessageResult<DataCollection<FavoriteDto>>>
        {
            private readonly IFavoriteRepository _favoriteRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Manejador(IFavoriteRepository favoriteRepository, IHttpContextAccessor httpContextAccessor)
            {
                _favoriteRepository = favoriteRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<MessageResult<DataCollection<FavoriteDto>>> Handle(FavoriteGetAllRequest request, CancellationToken cancellationToken)
            {
                // Obtener UserId desde el token
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim))
                    throw new ErrorHandler(HttpStatusCode.Unauthorized, "Usuario no autenticado.");

                int userId = int.Parse(userIdClaim);

                var (status, result, message) = await _favoriteRepository.GetAllFavorites(userId, cancellationToken);


                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<FavoriteDto>>.Of(message, result!);
            }
        }
    }
}
