using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Category.Command.Create.CategoryCreate;
using static Cinemax.Application.Features.Category.Command.Delete.CategoryDelete;
using static Cinemax.Application.Features.Category.Command.Update.CategoryUpdate;
using static Cinemax.Application.Features.Category.Queries.GetAll.CategoryQuery;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : MyBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(MessageResult<DataCollection<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<DataCollection<CategoryDto>>>> Get([FromQuery] CategoryQueryRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status201Created)]
        public async Task<ActionResult<MessageResult<int>>> Create([FromBody] CategoryCreateRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            request.Id = id;
            return await Mediator.Send(request);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MessageResult<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<MessageResult<int>>> Delete(int id)
        {
            return await Mediator.Send(new CategoryDeleteRequest { Id = id });
        }
    }
}
