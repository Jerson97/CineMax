using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Directors.Queries.GetAll
{
    public class DirectorQuery
    {
        public class DirectorQueryRequest : IRequest<MessageResult<DataCollection<DirectorDto>>> 
        {
            public int Page { get; set; } = 1;
            public int Amount { get; set; } = 5;
        }

        public class Manejador : IRequestHandler<DirectorQueryRequest, MessageResult<DataCollection<DirectorDto>>>
        {
            private readonly IDirectoryRepository _directoryRepository;

            public Manejador(IDirectoryRepository directoryRepository)
            {
                _directoryRepository = directoryRepository;
            }
            public async Task<MessageResult<DataCollection<DirectorDto>>> Handle(DirectorQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _directoryRepository.GetDirector(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<DirectorDto>>.Of(message, result);
            }
        }
    }
}
