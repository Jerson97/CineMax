using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;
using static Cinemax.Application.Features.Seasons.Queries.EpisodeBySeason.EpisodeBySeasonQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : MyBaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageResult<DataCollection<EpisodeDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<EpisodeDto>>>> GetById(int id)
        {
            return await Mediator.Send(new EpisodeBySeasonQueryRequest { Id = id });
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] SeasonCreateRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
