using AutoMapper;
using Blog.Core.Featuers.Comment.Query.Model;
using Blog.Core.Featuers.Comment.Query.Response;
using Blog.Core.Wrappers;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Query.Handler
{
    public class CommentQueryHandler : ReturnBaseHandler,
        IRequestHandler<GetPostCommentsQuery, ReturnBase<PaginatedResult<GetPostCommentsResponse>>>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentQueryHandler(ICommentService commentService, IMapper mapper)
        {
            this._commentService = commentService;
            this._mapper = mapper;
        }


        public async Task<ReturnBase<PaginatedResult<GetPostCommentsResponse>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getPostCommentsResult = await _commentService.GetPostCommentsAsync(request.PostId);
                if (!getPostCommentsResult.Succeeded)
                    return Failed<PaginatedResult<GetPostCommentsResponse>>(getPostCommentsResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetPostCommentsResponse>(getPostCommentsResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return Success(mappedResult);
            }
            catch (Exception ex)
            {
                return Failed<PaginatedResult<GetPostCommentsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
