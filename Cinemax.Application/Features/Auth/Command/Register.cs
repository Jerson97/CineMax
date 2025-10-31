using System.Net;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Auth.Command
{
    public class Register
    {
        public class RegisterRequest : IRequest<MessageResult<RegisterResponse>>
        {
            public string? Name { get; set; }
            public string? LastName { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class Manejador : IRequestHandler<RegisterRequest, MessageResult<RegisterResponse>>
        {
            private readonly IRegisterRepository _registerRepository;

            public Manejador(IRegisterRepository registerRepository)
            {
                _registerRepository = registerRepository;
            }

            public async Task<MessageResult<RegisterResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken)
            {
                var (status, registerResponse, message) = await _registerRepository.Register(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<RegisterResponse>.Of(message, registerResponse!);
            }
        }
    }
}
