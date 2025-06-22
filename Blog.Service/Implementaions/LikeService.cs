using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Implementaions
{
    internal class LikeService : ReturnBaseHandler, ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRespository _postRespository;

        public LikeService(ILikeRepository likeRepository, IPostRespository postRespository)
        {
            this._likeRepository = likeRepository;
            _postRespository = postRespository;
        }

        public async Task<ReturnBase<bool>> ToggleLikeAsync(Like like)
        {
            try
            {
                var isUserLikedThePost = await _likeRepository.GetTableNoTracking()
                    .Data.Where(x => x.UserId == like.UserId && x.PostId == like.PostId)
                    .FirstOrDefaultAsync();

                var postResult = await _postRespository.GetByIdAsync(like.PostId);

                if (!postResult.Succeeded)
                    return Failed<bool>("Invalid post id");

                if (isUserLikedThePost is null)
                {
                    var toggleLikeResult = await _likeRepository.AddAsync(like);
                    if (!toggleLikeResult.Succeeded)
                        return Failed<bool>(toggleLikeResult.Message);
                    postResult.Data.LikesCount++;
                }
                else
                {
                    var removeLikeResult = await _likeRepository.DeleteAsync(like.Id);
                    if (!removeLikeResult.Succeeded)
                        return Failed<bool>(removeLikeResult.Message);
                    postResult.Data.LikesCount--;
                }

                await _likeRepository.SaveChangesAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
