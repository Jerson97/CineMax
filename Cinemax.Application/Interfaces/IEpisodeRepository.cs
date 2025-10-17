using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Episodes.Command.Create.EpisodeCreate;

namespace Cinemax.Application.Interfaces
{
    public interface IEpisodeRepository
    {
        Task<(ServiceStatus status, int?, string message)> InsertEpisode(EpisodeCreateRequest request, CancellationToken cancellationToken);
    }
}
