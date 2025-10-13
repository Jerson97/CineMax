using System.Net;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Actors.Command.Create
{
    public class ActorCreate
    {
        public class ActorCreateRequest : IRequest<MessageResult<int>>
        {
            public string? Name { get; set; }
        }

        public class Manejador : IRequestHandler<ActorCreateRequest, MessageResult<int>>
        {
            private readonly IActorRepository _actorRepository;

            public Manejador(IActorRepository actorRepository)
            {
                _actorRepository = actorRepository;
            }
            public async Task<MessageResult<int>> Handle(ActorCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, actorId, message) = await _actorRepository.InsertActor(request, cancellationToken);

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
