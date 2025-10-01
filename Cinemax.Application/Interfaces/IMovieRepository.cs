using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Movies.Commands.Create;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Result;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Delete.MovieDelete;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;
using static Cinemax.Application.Features.Movies.Queries.GetAll.MovieQuery;

namespace Cinemax.Application.Interfaces
{
    public interface IMovieRepository
    {
        //Task<(ServiceStatus, MovieDto?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, DataCollection<MovieDto>, string)> GetMovie(MovieQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertMovie(MovieCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateMovie(MovieUpdateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> DeleteMovie(MovieDeleteRequest request, CancellationToken cancellationToken);
    }

}
