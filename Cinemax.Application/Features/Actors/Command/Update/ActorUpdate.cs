using System.Net;
using System.Text.Json.Serialization;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Actors.Command.Update
{
    public class ActorUpdate
    {
        public class ActorUpdateRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Name { get; set; }
        }
        public class Manejador : IRequestHandler<ActorUpdateRequest, MessageResult<int>>
        {
            private readonly IActorRepository _actorRepository;

            public Manejador(IActorRepository actorRepository)
            {
                _actorRepository = actorRepository;
            }
            public async Task<MessageResult<int>> Handle(ActorUpdateRequest request, CancellationToken cancellationToken)
            {
                var (status, actorId, message) = await _actorRepository.UpdateActor(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, actorId!.Value);
            }
        }
    }
}
