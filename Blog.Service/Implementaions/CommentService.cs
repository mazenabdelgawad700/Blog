using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Implementaions
{
    internal class CommentService : ReturnBaseHandler, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRespository _postRespository;

        public CommentService(ICommentRepository commentRepository, IPostRespository postRespository)
        {
            this._commentRepository = commentRepository;
            this._postRespository = postRespository;
        }
        public async Task<ReturnBase<bool>> AddCommentAsync(Comment comment)
        {
            var transaction = await _commentRepository.BeginTransactionAsync();
            try
            {
                var addCommentResult = await _commentRepository.AddAsync(comment);

                if (!addCommentResult.Succeeded)
                    return Failed<bool>(addCommentResult.Message);

                var postResult = await _postRespository.GetByIdAsync(comment.PostId);

                if (!postResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return Failed<bool>(postResult.Message);
                }

                postResult.Data.CommentsCount++;
                await _postRespository.SaveChangesAsync();

                await transaction.CommitAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<Comment>> GetCommentForUpdateAsync(int commentId)
        {
            try
            {
                var comment = await _commentRepository.GetTableNoTracking().Data.Where(x => x.Id == commentId).FirstOrDefaultAsync();

                if (comment is null)
                    return Failed<Comment>("Comment dose not exist");

                return Success(comment);
            }
            catch (Exception ex)
            {
                return Failed<Comment>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Comment>>> GetPostCommentsAsync(int postId)
        {
            try
            {
                var commentsResult = await _commentRepository.GetAllCommentsAsync(postId);

                if (!commentsResult.Succeeded)
                    return Failed<IQueryable<Comment>>("Comment dose not exist");

                return Success(commentsResult.Data!);
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<Comment>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateCommentAsync(Comment comment)
        {
            try
            {
                var updateCommentResult = await _commentRepository.UpdateAsync(comment);

                if (!updateCommentResult.Succeeded)
                    return Failed<bool>(updateCommentResult.Message);

                comment.UpdatedAt = DateTime.UtcNow;
                await _postRespository.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
