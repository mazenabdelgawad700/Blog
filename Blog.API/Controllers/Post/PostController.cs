using Blog.API.Base;
using Blog.Core.Featuers.Post.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.Post
{
    [Route("api/post/[action]")]
    public class PostController : AppControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] AddPostCommand command)
        {
            var Result = await Mediator.Send(command);
            return ReturnResult(Result);
        }
    }
}
