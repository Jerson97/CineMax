using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using static Cinemax.Application.Features.Actors.Queries.GetAll.ActorQuery;

namespace Cinemax.Application.Features.Actors.Queries.GetAll
{
    public class ActorQuery
    {
        public class ActorQueryRequest : IRequest<MessageResult<DataCollection<ActorDto>>>
        {
            public int Page { get; set; }
            public int Amount { get; set; }
        }
    }

    public class Manejador : IRequestHandler<ActorQueryRequest, MessageResult<DataCollection<ActorDto>>>
    {
        private readonly IActorRepository _actorRepository;

        public Manejador(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }
        public async Task<MessageResult<DataCollection<ActorDto>>> Handle(ActorQueryRequest request, CancellationToken cancellationToken)
        {
            var (status, result, message) = await _actorRepository.GetActor(request, cancellationToken);

            if (status != ServiceStatus.Ok)
                throw new ErrorHandler(
                    status == ServiceStatus.NotFound
                        ? System.Net.HttpStatusCode.NotFound
                        : System.Net.HttpStatusCode.InternalServerError,
                    message);

            return MessageResult<DataCollection<ActorDto>>.Of(message, result);
        }
    }
}
