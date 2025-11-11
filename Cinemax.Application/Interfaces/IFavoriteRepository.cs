using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Favorites.Command.Create.FavoriteCreate;
using static Cinemax.Application.Features.Favorites.Command.Delete.FavoriteDelete;

namespace Cinemax.Application.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<(ServiceStatus, DataCollection<FavoriteDto>?, string)>GetAllFavorites(int userId, CancellationToken cancellationToken);

        Task<(ServiceStatus, int?, string)> InsertFavorite(FavoriteCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> DeleteFavorite(FavoriteDeleteRequest request, CancellationToken cancellationToken);
    }
}
