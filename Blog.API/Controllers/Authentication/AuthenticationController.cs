using Blog.API.Base;
using Blog.Core.Featuers.Authentication.Command.Model;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.Authentication
{
    [Route("api/authentication/[action]")]
    public class AuthenticationController : AppControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
    }
}
