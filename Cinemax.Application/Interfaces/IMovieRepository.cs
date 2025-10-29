using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Delete.MovieDelete;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;
using static Cinemax.Application.Features.Movies.Queries.GetAll.MovieQuery;
using static Cinemax.Application.Features.Movies.Queries.GetById.MovieDetailQuery;
using static Cinemax.Application.Features.Movies.Queries.MovieByCategory.MovieByCategory;

namespace Cinemax.Application.Interfaces
{
    public interface IMovieRepository
    {
        //Task<(ServiceStatus, MovieDto?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, DataCollection<MovieDto>, string)> GetMovie(MovieQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, MovieByIdDto, string)> GetMovieId(MovieDetailQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, DataCollection<MovieDto>, string)> GetMovieByCategory(MovieByCategoryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateMovie(MovieUpdateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> DeleteMovie(MovieDeleteRequest request, CancellationToken cancellationToken);
    }

}
