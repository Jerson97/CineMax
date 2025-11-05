using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Search.Queries.SearchByName.SearchByNameQuery;

namespace Cinemax.Application.Interfaces
{
    public interface ISearchRepository
    {
        Task<(ServiceStatus, DataCollection<SearchByNameDto>, string)> SearchByName(SearchByNameQueryRequest request, CancellationToken cancellationToken);
    }
}
