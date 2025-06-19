using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Abstracts
{
    public interface IImageService
    {
        Task<ReturnBase<string>> SaveAsync(IFormFile file);
        ReturnBase<bool> Delete(string imgName);
    }
}
