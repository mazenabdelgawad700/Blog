using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface ICommentService
    {
        Task<ReturnBase<bool>> AddCommentAsync(Comment comment);
    }
}
