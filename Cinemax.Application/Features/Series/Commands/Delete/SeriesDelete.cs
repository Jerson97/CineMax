using System.Net;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Series.Commands.Delete
{
    public class SeriesDelete
    {
        public class SeriesDeleteRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<SeriesDeleteRequest, MessageResult<int>>
        {
            private readonly ISeriesRepository _seriesRepository;
            public Manejador(ISeriesRepository seriesRepository)
            {
                _seriesRepository = seriesRepository;
            }
            public async Task<MessageResult<int>> Handle(SeriesDeleteRequest request, CancellationToken cancellationToken)
            {
                var (status, seriesId, message) = await _seriesRepository.DeleteSeries(request, cancellationToken);
                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                        message);
                return MessageResult<int>.Of(message, seriesId!.Value);
            }
        }
    }
}
