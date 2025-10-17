using System.Net;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Episodes.Command.Create
{
    public class EpisodeCreate
    {
        public class EpisodeCreateRequest : IRequest<MessageResult<int>>
        {
            public int Number { get; set; }
            public string Title { get; set; } = string.Empty;
            public DateTime? ReleaseDate { get; set; }
            public int SeasonId { get; set; }
        }

        public class Manejador : IRequestHandler<EpisodeCreateRequest, MessageResult<int>>
        {
            private readonly IEpisodeRepository _episodeRepository;

            public Manejador(IEpisodeRepository episodeRepository)
            {
                _episodeRepository = episodeRepository;
            }
            public async Task<MessageResult<int>> Handle(EpisodeCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, episodeId, message) = await _episodeRepository.InsertEpisode(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, episodeId!.Value);
            }
        }
    }
}
