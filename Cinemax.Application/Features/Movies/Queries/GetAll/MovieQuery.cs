using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Features.Movies.Queries.GetAll
{
    public class MovieQuery
    {
        public class MovieQueryRequest : IRequest<MessageResult<DataCollection<MovieDto>>>
        {
            public string? Search { get; set; }
            public int Page { get; set; } = 1;
            public int Amount { get; set; } = 5;
        }

        public class Manejador : IRequestHandler<MovieQueryRequest, MessageResult<DataCollection<MovieDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IMovieRepository _movieRepository;

            public Manejador(IApplicationDbContext context, IMapper mapper, IMovieRepository movieRepository)
            {
                _context = context;
                _mapper = mapper;
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
