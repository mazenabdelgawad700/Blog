using Blog.Core.Featuers.Comment.Query.Response;
using Blog.Core.Wrappers;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Query.Model
{
    public class GetCommentRepliesQuery : IRequest<ReturnBase<PaginatedResult<GetCommentRepliesResponse>>>
    {
        public int ParentCommentId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
