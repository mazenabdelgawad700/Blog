using AutoMapper;
using Blog.Core.Featuers.Post.Query.Model;
using Blog.Core.Featuers.Post.Query.Response;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Query.Handler
{
    public class PostQueryHandler : ReturnBaseHandler
        , IRequestHandler<GetPostByIdQuery, ReturnBase<GetPostByIdResponse>>
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
    }
}
