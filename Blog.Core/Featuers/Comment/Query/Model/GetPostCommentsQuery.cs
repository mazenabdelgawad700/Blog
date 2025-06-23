using Blog.Core.Featuers.Comment.Query.Response;
using Blog.Core.Wrappers;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Query.Model
{
    public class GetPostCommentsQuery : IRequest<ReturnBase<PaginatedResult<GetPostCommentsResponse>>>
    {
        public int PostId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
