using AutoMapper;
using Blog.Core.Featuers.Post.Command.Model;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Post.Command.Handler
{
    public class PostCommandHandler : ReturnBaseHandler, IRequestHandler<AddPostCommand, ReturnBase<bool>>
    {
        private readonly IPostService _postService;
        private readonly IPostPictureService _postPictureService;
        private readonly IMapper _mapper;

        public PostCommandHandler(IPostService postService, IPostPictureService postPictureService, IMapper mapper)
        {
            _postService = postService;
            _postPictureService = postPictureService;
            _mapper = mapper;
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
    }
}
