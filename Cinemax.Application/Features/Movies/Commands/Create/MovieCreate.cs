using System.Net;
using AutoMapper;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Movies.Commands.Create
{
    public class MovieCreate
    {
        public class MovieCreateRequest : IRequest<MessageResult<int>>
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime ReleaseDate { get; set; }
            public List<int> CategoryIds { get; set; } = new();
            public int Duration { get; set; }

        }

        public class Manejador : IRequestHandler<MovieCreateRequest, MessageResult<int>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMovieRepository _movieRepository;
            private readonly IMapper _mapper;

            public Manejador(IApplicationDbContext context, IMapper mapper, IMovieRepository movieRepository)     
            {
                _context = context;
                _movieRepository = movieRepository;
                _mapper = mapper;
            }

            public async Task<MessageResult<int>> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, movieId, message) = await _movieRepository.InsertMovie(request, cancellationToken);

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
