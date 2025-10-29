using System.Net;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Features.Series.Commands.Create
{
    public class SeriesCreate
    {
        public class SeriesCreateRequest : IRequest<MessageResult<int>>
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public List<int> CategoryIds { get; set; } = new();
            public List<int> DirectorIds { get; set; } = new();
            public List<int> ActorIds { get; set; } = new();
            public IFormFile? Image { get; set; }
            public int Duration { get; set; }
        }

        public class Manejador : IRequestHandler<SeriesCreateRequest, MessageResult<int>>
        {
            private readonly ISeriesRepository _seriesRepository;

            public Manejador(ISeriesRepository seriesRepository)
            {
                _seriesRepository = seriesRepository;
            }
            public async Task<MessageResult<int>> Handle(SeriesCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, movieId, message) = await _seriesRepository.InsertSeries(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<int>.Of(message, movieId!.Value);
            }
        }
    }
}
