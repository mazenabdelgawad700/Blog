using AutoMapper;
using Blog.Core.Featuers.Post.Query.Model;
using Blog.Core.Featuers.Post.Query.Response;
using Blog.Core.Wrappers;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Query.Handler
{
    public class PostQueryHandler : ReturnBaseHandler
        , IRequestHandler<GetPostByIdQuery, ReturnBase<GetPostByIdResponse>>
        , IRequestHandler<GetPostsQuery, ReturnBase<PaginatedResult<GetPostsResponse>>>
        , IRequestHandler<GetUserPostsQuery, ReturnBase<PaginatedResult<GetUserPostsResponse>>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostQueryHandler(IPostService postService, IMapper _mapper)
        {
            this._postService = postService;
            this._mapper = _mapper;
        }


        public async Task<ReturnBase<GetPostByIdResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getPostResult = await _postService.GetPostByIdAsync(request.Id);
                if (!getPostResult.Succeeded)
                    return Failed<GetPostByIdResponse>(getPostResult.Message);

                var mappedResult = _mapper.Map<GetPostByIdResponse>(getPostResult.Data);

                foreach (var like in mappedResult.Likes)
                {
                    if (like.UserId == request.UserId)
                    {
                        mappedResult.IsLikedByCurrentUser = true;
                        break;
                    }
                }

                return Success(mappedResult);
            }
            catch (Exception ex)
            {
                return Failed<GetPostByIdResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<PaginatedResult<GetPostsResponse>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getPostsResult = await _postService.GetPostsAsync();

                if (!getPostsResult.Succeeded)
                    return Failed<PaginatedResult<GetPostsResponse>>(getPostsResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetPostsResponse>(getPostsResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return Success(mappedResult);
            }
            catch (Exception ex)
            {
                return Failed<PaginatedResult<GetPostsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<PaginatedResult<GetUserPostsResponse>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserPostsResult = await _postService.GetUserPostsAsync(request.Id);

                if (!getUserPostsResult.Succeeded)
                    return Failed<PaginatedResult<GetUserPostsResponse>>(getUserPostsResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetUserPostsResponse>(getUserPostsResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return Success(mappedResult);
            }
            catch (Exception ex)
            {
                return Failed<PaginatedResult<GetUserPostsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
