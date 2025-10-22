using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;
using static Cinemax.Application.Features.Seasons.Queries.EpisodeBySeason.EpisodeBySeasonQuery;

namespace Cinemax.Application.Interfaces
{
    public interface ISeasonRepository
    {
        Task<(ServiceStatus, DataCollection<EpisodeDto>, string)> GetEpisodeBySeason(EpisodeBySeasonQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertSeason(SeasonCreateRequest request, CancellationToken cancellationToken); 
    }
}
