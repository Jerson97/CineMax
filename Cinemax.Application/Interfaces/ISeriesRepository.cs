using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;

namespace Cinemax.Application.Interfaces
{
    public interface ISeriesRepository
    {
        Task<(ServiceStatus, DataCollection<SeriesDto>, string)> GetSeries(SeriesQueryRequest request, CancellationToken cancellationToken);
        //Task<(ServiceStatus, MovieDto, string)> GetMovieId(MovieDetailQueryRequest request, CancellationToken cancellationToken);
        //Task<(ServiceStatus, DataCollection<MovieDto>, string)> GetMovieByCategory(MovieByCategoryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertSeries(SeriesCreateRequest request, CancellationToken cancellationToken);
        //Task<(ServiceStatus, int?, string)> UpdateMovie(MovieUpdateRequest request, CancellationToken cancellationToken);
        //Task<(ServiceStatus, int?, string)> DeleteMovie(MovieDeleteRequest request, CancellationToken cancellationToken);
    }
}
