using Blog.API.Base;
using Blog.Core.Featuers.Post.Command.Model;
using Blog.Core.Featuers.Post.Query.Model;
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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdatePostCommand command)
        {
            var Result = await Mediator.Send(command);
            return ReturnResult(Result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetPostByIdQuery query)
        {
            var Result = await Mediator.Send(query);
            return ReturnResult(Result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] GetPostsQuery query)
        {
            var Result = await Mediator.Send(query);
            return ReturnResult(Result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPosts([FromQuery] GetUserPostsQuery query)
        {
            var Result = await Mediator.Send(query);
            return ReturnResult(Result);
        }
    }
}
