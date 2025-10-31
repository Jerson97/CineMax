using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.Application.Interfaces
{
    public interface IRegisterRepository
    {
        Task<(ServiceStatus, RegisterResponse?, string)> Register(RegisterRequest request, CancellationToken cancellationToken);
    }
}
