using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Seasons.Queries.EpisodeBySeason
{
    public class EpisodeBySeasonQuery
    {
        public class EpisodeBySeasonQueryRequest : IRequest<MessageResult<DataCollection<EpisodeDto>>>
        {
            public int Id { get; set; }
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class Manejador : IRequestHandler<EpisodeBySeasonQueryRequest, MessageResult<DataCollection<EpisodeDto>>>
        {
            private readonly ISeasonRepository _seasonRepository;

            public Manejador(ISeasonRepository seasonRepository)
            {
                _seasonRepository = seasonRepository;
            }
            public async Task<MessageResult<DataCollection<EpisodeDto>>> Handle(EpisodeBySeasonQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _seasonRepository.GetEpisodeBySeason(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<EpisodeDto>>.Of(message, result);
            }
        }
    }
}
