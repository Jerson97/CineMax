using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Search.Queries.SearchByName.SearchByNameQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<SearchByNameDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<SearchByNameDto>>>> Get([FromQuery] SearchByNameQueryRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
