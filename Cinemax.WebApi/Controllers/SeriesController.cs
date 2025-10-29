using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Commands.Delete.SeriesDelete;
using static Cinemax.Application.Features.Series.Commands.Update.SeriesUpdate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;
using static Cinemax.Application.Features.Series.Queries.GetById.SerieDetailQuery;
using static Cinemax.Application.Features.Series.Queries.SeriesByCategory.SerieByCategoryQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/series")]
    [ApiController]
    public class SeriesController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<SeriesDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<SeriesDto>>>> Get([FromQuery] SeriesQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageResult<SerieByIdDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<SerieByIdDto>>> GetById(int id)
        {

            return await Mediator.Send(new SerieDetailQueryRequest { Id = id });
        }

        [HttpGet("by-category/{id}")]
        [ProducesResponseType(typeof(MessageResult<DataCollection<SeriesDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<SeriesDto>>>> GetSeriesByCategory(int id, [FromQuery] SerieByCategoryQueryRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);

        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromForm] SeriesCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update(int id, [FromForm] SeriesUpdateRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Delete(int id)
        {
            return await Mediator.Send(new SeriesDeleteRequest { Id = id });
        }
    }
}
