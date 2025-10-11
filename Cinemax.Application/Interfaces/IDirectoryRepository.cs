using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;
using static Cinemax.Application.Features.Directors.Command.Update.DirectorUpdate;
using static Cinemax.Application.Features.Directors.Queries.GetAll.DirectorQuery;

namespace Cinemax.Application.Interfaces
{
    public interface IDirectoryRepository
    {
        Task<(ServiceStatus, DataCollection<DirectorDto>, string)> GetDirector(DirectorQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertDirector(DirectorCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateDirector(DirectorUpdateRequest request, CancellationToken cancellationToken);
    }
}
