using AutoMapper;
using Blog.Core.Featuers.Post.Command.Model;
using Blog.Domain.Entities;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Command.Handler
{
    public class PostCommandHandler : ReturnBaseHandler,
        IRequestHandler<AddPostCommand, ReturnBase<bool>>,
        IRequestHandler<UpdatePostCommand, ReturnBase<bool>>,
        IRequestHandler<ToggleLikeButtonCommand, ReturnBase<bool>>
    {
        private readonly IPostService _postService;
        private readonly IPostPictureService _postPictureService;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;

        public PostCommandHandler(IPostService postService, IPostPictureService postPictureService, IMapper mapper, ILikeService likeService)
        {
            _postService = postService;
            _postPictureService = postPictureService;
            _mapper = mapper;
            _likeService = likeService;
        }

        public async Task<ReturnBase<bool>> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mappedResult = _mapper.Map<Domain.Entities.Post>(request);

                var savePostResult = await _postService.AddPostAsync(mappedResult);

                if (request.Images.Count > 0)
                {
                    var saveImagesResult = await _postPictureService.AddPostImagesAsync(request.Images, savePostResult.Data);
                    if (!saveImagesResult.Succeeded)
                        return Failed<bool>(saveImagesResult.Message);
                }

                return Success(true, savePostResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var postResult = await _postService.GetPostForUpdateAsync(request.Id);

                if (!postResult.Succeeded)
                    return Failed<bool>(postResult.Message);


                if (request.Content is not null)
                    postResult.Data.Content = request.Content;

                if (request.Title is not null)
                    postResult.Data.Title = request.Title;

                if (request.Summary is not null)
                    postResult.Data.Summary = request.Summary;


                var updatePostResult = await _postService.UpdatePostAsync(postResult.Data);

                if (!updatePostResult.Succeeded)
                    return Failed<bool>(updatePostResult.Message);

                var updatePostPicturesResult = await _postPictureService.UpdatePostImagesAsync(request.NewImages, request.OldImages, request.Id);

                if (!updatePostPicturesResult.Succeeded)
                    return Failed<bool>(updatePostPicturesResult.Message);

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ToggleLikeButtonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mappedResult = _mapper.Map<Like>(request);
                var toggleLikeResult = await _likeService.ToggleLikeAsync(mappedResult);

                if (!toggleLikeResult.Succeeded)
                    return Failed<bool>(toggleLikeResult.Message);

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
