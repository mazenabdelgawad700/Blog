using Blog.API.Base;
using Blog.Core.Featuers.Comment.Command.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.Comment
{
    [Route("api/comment/[action]")]
    public class CommentController : AppControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddCommentCommand command)
        {
            var result = await Mediator.Send(command);
            return ReturnResult(result);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateCommentCommand command)
        {
            var result = await Mediator.Send(command);
            return ReturnResult(result);
        }
    }
}
