using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using System.Net;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Seasons.Command.Create
{
    public class SeasonCreate
    {
        public class SeasonCreateRequest : IRequest<MessageResult<int>>
        {
            public int Number { get; set; }
            public int SeriesId { get; set; }
        }

        public class Manejador : IRequestHandler<SeasonCreateRequest, MessageResult<int>>
        {
            private readonly ISeasonRepository _seasonRepository;

            public Manejador(ISeasonRepository seasonRepository)
            {
                _seasonRepository = seasonRepository;
            }
            public async Task<MessageResult<int>> Handle(SeasonCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, seasonId, message) = await _seasonRepository.InsertSeason(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, seasonId!.Value);
            }
        }
    }
}
