using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Actors.Command.Create.ActorCreate;
using static Cinemax.Application.Features.Actors.Command.Update.ActorUpdate;
using static Cinemax.Application.Features.Actors.Queries.GetAll.ActorQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<ActorDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<ActorDto>>>> Get([FromQuery] ActorQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] ActorCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update(int id, [FromBody] ActorUpdateRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }
    }
}
