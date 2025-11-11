using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Favorites.Command.Create.FavoriteCreate;
using static Cinemax.Application.Features.Favorites.Command.Delete.FavoriteDelete;
using static Cinemax.Application.Features.Favorites.Queries.GetAll.FavoriteGetAll;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoriteController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<FavoriteDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResult<DataCollection<FavoriteDto>>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResult<DataCollection<FavoriteDto>>>> GetAll()
        {
            return await Mediator.Send(new FavoriteGetAllRequest());
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] FavoriteCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageResult<int>>> Delete([FromBody] FavoriteDeleteRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
