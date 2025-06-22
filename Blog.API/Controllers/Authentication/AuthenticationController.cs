using Blog.API.Base;
using Blog.Core.Featuers.Authentication.Command.Model;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] SendResetPasswordEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
    }
}
