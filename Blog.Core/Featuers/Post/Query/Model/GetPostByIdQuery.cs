using Blog.Core.Featuers.Post.Query.Response;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Query.Model
{
    public class GetPostByIdQuery : IRequest<ReturnBase<GetPostByIdResponse>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
