using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Actors.Command.Create.ActorCreate;
using static Cinemax.Application.Features.Actors.Command.Update.ActorUpdate;
using static Cinemax.Application.Features.Actors.Queries.GetAll.ActorQuery;

namespace Cinemax.Application.Interfaces
{
    public interface IActorRepository
    {
        Task<(ServiceStatus, DataCollection<ActorDto>, string)> GetActor(ActorQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertActor(ActorCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateActor(ActorUpdateRequest request, CancellationToken cancellationToken);
    }
}
