using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Auth.Command.Login;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<(ServiceStatus, UserResponse?, string)> Register(RegisterRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, UserResponse?, string)> Login(LoginRequest request, CancellationToken cancellationToken);
    }
}
