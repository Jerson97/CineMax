using Cinemax.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Auth.Command.Login;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : MyBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest parametros) => Ok(await Mediator.Send(parametros));

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterRequest parametros) => Ok(await Mediator.Send(parametros));
    }
}
 