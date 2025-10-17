using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Commands.Delete.SeriesDelete;
using static Cinemax.Application.Features.Series.Commands.Update.SeriesUpdate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;
using static Cinemax.Application.Features.Series.Queries.GetById.SerieDetailQuery;
using static Cinemax.Application.Features.Series.Queries.SeriesByCategory.SerieByCategoryQuery;

namespace Cinemax.Application.Interfaces
{
    public interface ISeriesRepository
    {
        Task<(ServiceStatus, DataCollection<SeriesDto>, string)> GetSeries(SeriesQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, SeriesDto, string)> GetSerieId(SerieDetailQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, DataCollection<SeriesDto>, string)> GetSerieByCategory(SerieByCategoryQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertSeries(SeriesCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateSeries(SeriesUpdateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> DeleteSeries(SeriesDeleteRequest request, CancellationToken cancellationToken);
    }
}
