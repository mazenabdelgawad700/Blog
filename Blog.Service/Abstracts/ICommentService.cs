using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface ICommentService
    {
        Task<ReturnBase<bool>> AddCommentAsync(Comment comment);
        Task<ReturnBase<bool>> UpdateCommentAsync(Comment comment);
        Task<ReturnBase<Comment>> GetCommentForUpdateAsync(int commentId);
        Task<ReturnBase<IQueryable<Comment>>> GetPostCommentsAsync(int postId);
        Task<ReturnBase<IQueryable<Comment>>> GetCommentRepliesAsync(int commentId);
    }
}
