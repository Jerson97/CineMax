using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;
using static Cinemax.Application.Features.Directors.Command.Update.DirectorUpdate;
using static Cinemax.Application.Features.Directors.Queries.GetAll.DirectorQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<DirectorDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<DirectorDto>>>> Get([FromQuery] DirectorQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] DirectorCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update(int id, [FromBody] DirectorUpdateRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }
    }
}
