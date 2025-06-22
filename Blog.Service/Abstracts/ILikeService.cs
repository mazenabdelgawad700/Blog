using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface ILikeService
    {
        Task<ReturnBase<bool>> ToggleLikeAsync(Like like);
    }
}
