using ErsaCommerce.Application;
using ErsaCommerce.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ErsaCommerce.Api
{
    public class AuthController : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            var userId = await Mediator.Send(command);
            return Ok(new { userId });
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginDto>>> Login([FromBody] LoginCommand loginModel)
        {
            var loginResponse = await Mediator.Send(loginModel);
            return Ok(loginResponse);
        }
    }
}
