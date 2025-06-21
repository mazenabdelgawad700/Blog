using Blog.API.Base;
using Blog.Core.Featuers.ApplicationUserFeatuer.Command.Model;
using Blog.Core.Featuers.ApplicationUserFeatuer.Query.Model;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.ApplicationUser
{
    [Route("api/applicationuser/[action]")]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateApplicationUserProfileCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserProfileById([FromQuery] GetApplicationUserProfileByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return ReturnResult(response);
        }
    }
}
