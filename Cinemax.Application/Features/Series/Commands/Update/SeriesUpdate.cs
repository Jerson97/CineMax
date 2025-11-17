using System.Net;
using System.Text.Json.Serialization;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Features.Series.Commands.Update
{
    public class SeriesUpdate
    {
        public class SeriesUpdateRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime ReleaseDate { get; set; }
            public List<int> CategoryIds { get; set; } = new();
            public List<int> DirectorIds { get; set; } = new();
            public List<int> ActorIds { get; set; } = new();
            public IFormFile? Image { get; set; }
            public string? TrailerUrl { get; set; }
        }

        public class Manejador : IRequestHandler<SeriesUpdateRequest, MessageResult<int>>
        {
            private readonly ISeriesRepository _seriesRepository;

            public Manejador(ISeriesRepository seriesRepository)
            {
                _seriesRepository = seriesRepository;
            }
            public async Task<MessageResult<int>> Handle(SeriesUpdateRequest request, CancellationToken cancellationToken)
            {
                var (status, serieId, message) = await _seriesRepository.UpdateSeries(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<int>.Of(message, serieId!.Value);
            }
        }
    }
}
