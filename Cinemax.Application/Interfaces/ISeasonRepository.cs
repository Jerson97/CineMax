using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;

namespace Cinemax.Application.Interfaces
{
    public interface ISeasonRepository
    {
        Task<(ServiceStatus, int?, string)> InsertSeason(SeasonCreateRequest request, CancellationToken cancellationToken); 
    }
}
