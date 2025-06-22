using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IPostService
    {
        Task<ReturnBase<int>> AddPostAsync(Post post);
    }
}
