using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Movies.Commands.Create;
using Cinemax.Application.Features.Movies.Queries.GetAll;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;
using static Cinemax.Application.Features.Movies.Queries.GetAll.MovieQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<MovieDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<MovieDto>>>> Get([FromQuery] MovieQueryRequest request)
        {
            return await Mediator.Send(request);
        }
    

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] MovieCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update([FromBody] MovieUpdateRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
