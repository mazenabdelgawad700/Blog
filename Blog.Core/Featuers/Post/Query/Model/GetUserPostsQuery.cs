using Blog.Core.Featuers.Post.Query.Response;
using Blog.Core.Wrappers;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Query.Model
{
    public class GetUserPostsQuery : IRequest<ReturnBase<PaginatedResult<GetUserPostsResponse>>>
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
