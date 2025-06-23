using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;

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
    }
}
