using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Series.Queries.SeriesByCategory
{
    public class SerieByCategoryQuery
    {
        public class SerieByCategoryQueryRequest : IRequest<MessageResult<DataCollection<SeriesDto>>>
        {
            public int Id { get; set; }
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class Manejador : IRequestHandler<SerieByCategoryQueryRequest, MessageResult<DataCollection<SeriesDto>>>
        {
            private readonly ISeriesRepository _seriesRepository;

            public Manejador(ISeriesRepository seriesRepository)
            {
                _seriesRepository = seriesRepository;
            }
            public async Task<MessageResult<DataCollection<SeriesDto>>> Handle(SerieByCategoryQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _seriesRepository.GetSerieByCategory(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<SeriesDto>>.Of(message, result);
            }
        }
    }
}
