using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Implementaions
{
    internal class PostPictureService : ReturnBaseHandler, IPostPictureService
    {
        private readonly IPostPictureRespository _postPictureRespository;
        private IImageService _imageService;

        public PostPictureService(IImageService imageService, IPostPictureRespository postPictureRespository)
        {
            this._imageService = imageService;
            this._postPictureRespository = postPictureRespository;
        }


        public async Task<ReturnBase<bool>> AddPostImagesAsync(List<IFormFile> images, int postId)
        {
            try
            {
                byte counter = 0;
                foreach (var image in images)
                {
                    var saveImageResult = await _imageService.SaveAsync(image);

                    if (!saveImageResult.Succeeded)
                        return Failed<bool>(saveImageResult.Message);

                    PostPicture entity = new PostPicture()
                    {
                        PictureUrl = saveImageResult.Data,
                        DisplayOrder = ++counter,
                        PostId = postId
                    };


                    var addImagesResult = await _postPictureRespository.AddAsync(entity);
                    if (!addImagesResult.Succeeded)
                        return Failed<bool>(addImagesResult.Message);

                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
