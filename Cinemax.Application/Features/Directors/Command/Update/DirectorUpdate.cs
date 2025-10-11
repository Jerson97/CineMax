using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Directors.Command.Update
{
    public class DirectorUpdate
    {
        public class DirectorUpdateRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class Manejador : IRequestHandler<DirectorUpdateRequest, MessageResult<int>>
        {
            private readonly IDirectoryRepository _directoryRepository;

            public Manejador(IDirectoryRepository directoryRepository)
            {
                _directoryRepository = directoryRepository;
            }
            public async Task<MessageResult<int>> Handle(DirectorUpdateRequest request, CancellationToken cancellationToken)
            {
                var (status, directorId, message) = await _directoryRepository.UpdateDirector(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, directorId!.Value);
            }
        }
    }
}
