using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Command.Model
{
    public class DeletePostCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
    }
}
