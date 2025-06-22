using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ReturnBase<bool>> AddPostImagesForUpdateAsync(List<IFormFile> images, int postId)
        {
            try
            {
                var lastImage = await _postPictureRespository.GetTableNoTracking().Data.Where(x => x.PostId == postId).OrderBy(x => x.DisplayOrder).LastOrDefaultAsync();

                if (lastImage is null)
                    return Failed<bool>("please, try again");

                int counter = lastImage.DisplayOrder;
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
        public async Task<ReturnBase<bool>> UpdatePostImagesAsync(List<IFormFile> newImages, List<string> oldImages, int postId)
        {

            if (postId <= 0)
                return Failed<bool>("Invalid post id");

            if (oldImages is not null)
            {
                foreach (var image in oldImages)
                {
                    var deleteImageFromServerResult = _imageService.Delete(image);
                    if (!deleteImageFromServerResult.Succeeded)
                        return Failed<bool>($"Can not delete image: {image}");

                    var deleteImageFromDb = await _postPictureRespository.DeleteImageByNameAsync(image);
                    if (!deleteImageFromDb.Succeeded)
                        return Failed<bool>(deleteImageFromDb.Message);
                }
            }

            if (newImages is not null)
            {
                var addNewImageResult = await AddPostImagesForUpdateAsync(newImages, postId);
                if (!addNewImageResult.Succeeded)
                    return Failed<bool>(addNewImageResult.Message);
            }

            return Success(true);
        }
    }
}
