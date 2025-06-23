using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Command.Model
{
    public class DeleteCommentCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
