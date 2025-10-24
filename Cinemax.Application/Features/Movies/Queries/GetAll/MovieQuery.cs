using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Queries.GetAll
{
    public class MovieQuery
    {
        public class MovieQueryRequest : IRequest<MessageResult<DataCollection<MovieDto>>>
        {
            public string? Search { get; set; }
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class Manejador : IRequestHandler<MovieQueryRequest, MessageResult<DataCollection<MovieDto>>>
        {
            private readonly IMovieRepository _movieRepository;

            public Manejador(IMovieRepository movieRepository)
            {
                _movieRepository = movieRepository;
            }

            public async Task<MessageResult<DataCollection<MovieDto>>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _movieRepository.GetMovie(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<MovieDto>>.Of(message, result);
            }
        }
    }
}
