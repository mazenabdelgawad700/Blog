using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Abstracts
{
    public interface IPostPictureService
    {
        Task<ReturnBase<bool>> AddPostImagesAsync(List<IFormFile> images, int postId);
        Task<ReturnBase<bool>> UpdatePostImagesAsync(List<IFormFile> newImages, List<string> oldImages, int postId);
    }
}
