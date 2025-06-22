using Blog.Core.Featuers.Post.Query.Response;
using Blog.Core.Wrappers;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Query.Model
{
    public class GetPostsQuery : IRequest<ReturnBase<PaginatedResult<GetPostsResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
