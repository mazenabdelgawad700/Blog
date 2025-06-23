using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    internal class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Comment> _comments;

        public CommentRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._comments = _dbContext.Set<Comment>();
        }

        public async Task<ReturnBase<IQueryable<Comment>>> GetAllCommentsAsync(int postId)
        {
            try
            {
                var comments = _comments
                                .Include(x => x.User)
                                .Include(x => x.Replies)
                                .Where(x => x.PostId == postId && x.ParentCommentId == null && !x.IsDeleted)
                                .OrderBy(x => x.CreatedAt)
                                .AsQueryable();

                return await Task.FromResult(Success(comments));
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<Comment>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Comment>>> GetCommentRepliesAsync(int commentId)
        {
            try
            {
                var comments = _comments
                                .Include(x => x.User)
                                .Include(x => x.Replies)
                                .Where(x => x.ParentCommentId == commentId && !x.IsDeleted)
                                .OrderBy(x => x.CreatedAt)
                                .AsQueryable();

                return await Task.FromResult(Success(comments));
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<Comment>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
