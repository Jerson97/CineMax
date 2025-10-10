using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Queries.GetById
{
    public class MovieDetailQuery
    {
        public class MovieDetailQueryRequest : IRequest<MessageResult<MovieDto>>
        {
            public int Id { get; set; }
            public int Page { get; set; } = 1;
            public int Amount { get; set; } = 5;
        }

        public class Manejador : IRequestHandler<MovieDetailQueryRequest, MessageResult<MovieDto>>
        {
            private readonly IMovieRepository _movieRepository;
            public Manejador(IMovieRepository movieRepository)
            {
                _movieRepository = movieRepository;
            }
            public async Task<MessageResult<MovieDto>> Handle(MovieDetailQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _movieRepository.GetMovieId(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<MovieDto>.Of(message, result);
            }
        }
    }
}
