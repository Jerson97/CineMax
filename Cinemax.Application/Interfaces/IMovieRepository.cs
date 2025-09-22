using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Movies.Commands.Create;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Result;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;

namespace Cinemax.Application.Interfaces
{
    public interface IMovieRepository
    {
        //Task<(ServiceStatus, MovieDto?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
    }

}
