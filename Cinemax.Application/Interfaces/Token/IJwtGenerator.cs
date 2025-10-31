using CineMax.Domain.Entities;

namespace Cinemax.Application.Interfaces.Token
{
    public interface IJwtGenerator
    {
        Task<string> CreateToken(User user);
    }
}
