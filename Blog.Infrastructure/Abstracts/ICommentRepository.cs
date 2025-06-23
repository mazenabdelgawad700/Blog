using Blog.Domain.Entities;
using Blog.Domain.IBaseRepository;
using Blog.Shared.Base;

namespace Blog.Infrastructure.Abstracts
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<ReturnBase<IQueryable<Comment>>> GetAllCommentsAsync(int postId);
        Task<ReturnBase<IQueryable<Comment>>> GetCommentRepliesAsync(int commentId);
    }
}
