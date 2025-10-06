using System.Net;
using AutoMapper;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Commands.Delete
{
    public class MovieDelete
    {
        public class MovieDeleteRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<MovieDeleteRequest, MessageResult<int>>
        {
            private readonly IMovieRepository _movieRepository;

            public Manejador(IMapper mapper, IMovieRepository movieRepository)
            {
                _movieRepository = movieRepository;
            }
            public async Task<MessageResult<int>> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
            {
                var (status, movieId, message) = await _movieRepository.DeleteMovie(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<int>.Of(message, movieId!.Value);
            }
        }
    }
}
