using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Search.Queries.SearchByName
{
    public class SearchByNameQuery
    {
        public class SearchByNameQueryRequest : IRequest<MessageResult<DataCollection<SearchByNameDto>>>
        {
            public string? Search { get; set; }
            public string? Type { get; set; }
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class Manejador : IRequestHandler<SearchByNameQueryRequest, MessageResult<DataCollection<SearchByNameDto>>>
        {
            private readonly ISearchRepository _searchRepository;

            public Manejador(ISearchRepository searchRepository)
            {
                _searchRepository = searchRepository;
            }

            public async Task<MessageResult<DataCollection<SearchByNameDto>>> Handle(SearchByNameQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _searchRepository.SearchByName(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<SearchByNameDto>>.Of(message, result);
            }
        }
    }
}
