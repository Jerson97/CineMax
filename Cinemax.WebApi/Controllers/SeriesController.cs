using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;
using static Cinemax.Application.Features.Series.Queries.GetAll.SeriesQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<SeriesDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<SeriesDto>>>> Get([FromQuery] SeriesQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] SeriesCreateRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
