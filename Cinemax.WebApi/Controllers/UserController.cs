using Cinemax.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : MyBaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterRequest parametros) => Ok(await Mediator.Send(parametros));
    }
}
 