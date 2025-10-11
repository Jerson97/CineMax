using System.Net;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Directors.Command.Create
{
    public class DirectorCreate
    {
        public class DirectorCreateRequest : IRequest<MessageResult<int>>
        {
            public string? Name { get; set; }
        }

        public class Manejador : IRequestHandler<DirectorCreateRequest, MessageResult<int>>
        {
            private readonly IDirectoryRepository _directoryRepository;

            public Manejador(IDirectoryRepository directoryRepository)
            {
                _directoryRepository = directoryRepository;
            }
            public async Task<MessageResult<int>> Handle(DirectorCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, categoryId, message) = await _directoryRepository.InsertDirector(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, categoryId!.Value);
            }
        }
    }
}
