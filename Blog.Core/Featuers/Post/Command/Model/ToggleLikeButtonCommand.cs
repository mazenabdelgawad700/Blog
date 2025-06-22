using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Command.Model
{
    public class ToggleLikeButtonCommand : IRequest<ReturnBase<bool>>
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}
