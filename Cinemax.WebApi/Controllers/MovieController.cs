using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Delete.MovieDelete;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;
using static Cinemax.Application.Features.Movies.Queries.GetAll.MovieQuery;
using static Cinemax.Application.Features.Movies.Queries.GetById.MovieDetailQuery;
using static Cinemax.Application.Features.Movies.Queries.MovieByCategory.MovieByCategory;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<MovieDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<MovieDto>>>> Get([FromQuery] MovieQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageResult<MovieDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<MovieDto>>> GetById(int id)
        {

            return await Mediator.Send(new MovieDetailQueryRequest { Id = id});
        }

        [HttpGet("by-category/{id}")]
        [ProducesResponseType(typeof(MessageResult<DataCollection<MovieDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<MovieDto>>>> GeMovieByCategory(int id, [FromQuery] MovieByCategoryRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);

        }


        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] MovieCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update(int id, [FromBody] MovieUpdateRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Delete(int id)
        {
            return await Mediator.Send(new MovieDeleteRequest { Id = id });
        }

    }
}   
