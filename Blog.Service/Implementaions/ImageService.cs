using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Implementaions
{
    internal class ImageService : ReturnBaseHandler, IImageService
    {
        public async Task<ReturnBase<string>> SaveAsync(IFormFile file)
        {
            string contentPath = @"D:\BlogImages\";

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            string ext = Path.GetExtension(file.FileName);

            string fileName = $"{Guid.NewGuid()}{ext}";
            string fileNameWithPath = Path.Combine(contentPath, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            await file.CopyToAsync(stream);

            return Success(fileNameWithPath, "Image Saved Successfully");
        }
        public ReturnBase<bool> Delete(string imgFullPath)
        {
            //string imgName = Path.GetFileName(imgFullPath);
            if (string.IsNullOrEmpty(imgFullPath))
                return Failed<bool>($"file({nameof(imgFullPath)}) is null");

            //string contentPath = @"D:\BlogImages\";
            //string path = Path.Combine(contentPath, imgName);

            if (!File.Exists(imgFullPath))
                return Failed<bool>("Image dose not exist");

            File.Delete(imgFullPath);
            return Success(true, "Image Deleted Successfully");
        }
    }
}
