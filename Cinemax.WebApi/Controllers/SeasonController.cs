using CineMax.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : MyBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] SeasonCreateRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
