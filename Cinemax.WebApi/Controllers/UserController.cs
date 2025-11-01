using Cinemax.Application.DTOs;
using CineMax.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using static Cinemax.Application.Features.Auth.Command.Login;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : MyBaseController
    {
        [ProducesResponseType(typeof(MessageResult<UserResponse>), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest parametros) => Ok(await Mediator.Send(parametros));

        [ProducesResponseType(typeof(MessageResult<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterRequest parametros) => Ok(await Mediator.Send(parametros));
    }
}
 