using System.Net;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Auth.Command
{
    public class Login
    {
        public class LoginRequest : IRequest<MessageResult<UserResponse>>
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class Manejador : IRequestHandler<LoginRequest, MessageResult<UserResponse>>
        {
            private readonly IUserRepository _userRepository;

            public Manejador(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<MessageResult<UserResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
            {
                var (status, loginResponse, message) = await _userRepository.Login(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<UserResponse>.Of(message, loginResponse!);
            }
        }
    }
}
