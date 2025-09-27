using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Commands.Update
{
    public class MovieUpdate
    {
        public class MovieUpdateRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime ReleaseDate { get; set; }
            public List<int> CategoryIds { get; set; } = new();
            public int Duration { get; set; }
        }

        public class Manejador : IRequestHandler<MovieUpdateRequest, MessageResult<int>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMovieRepository _movieRepository;

            public Manejador(IApplicationDbContext context, IMapper mapper, IMovieRepository movieRepository)
            {
                _context = context;
                _movieRepository = movieRepository;
            }
            public async Task<MessageResult<int>> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
            {
                var (status, movieId, message) = await _movieRepository.UpdateMovie(request, cancellationToken);

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
