using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Episodes.Command.Create.EpisodeCreate;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController : MyBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] EpisodeCreateRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
