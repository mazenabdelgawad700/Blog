using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Command.Model
{
    public class AddCommentCommand : IRequest<ReturnBase<bool>>
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
