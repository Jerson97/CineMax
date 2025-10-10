using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Queries.MovieByCategory
{
    public class MovieByCategory
    {
        public class MovieByCategoryRequest : IRequest<MessageResult<DataCollection<MovieDto>>>
        {
            public int Id { get; set; }
            public int Page { get; set; } = 1;
            public int Amount { get; set; } = 5;
        }

        public class Manejador : IRequestHandler<MovieByCategoryRequest, MessageResult<DataCollection<MovieDto>>>
        {
            private readonly IMovieRepository _movieRepository;
            public Manejador(IMovieRepository movieRepository)
            {
                _movieRepository = movieRepository;
            }
            public async Task<MessageResult<DataCollection<MovieDto>>> Handle(MovieByCategoryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _movieRepository.GetMovieByCategory(request, cancellationToken);

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
