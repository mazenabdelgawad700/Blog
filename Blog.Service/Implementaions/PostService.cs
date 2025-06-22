using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Implementaions
{
    internal class PostService : ReturnBaseHandler, IPostService
    {

        private readonly IPostRespository _postRespository;

        public PostService(IPostRespository postRespository)
        {
            this._postRespository = postRespository;
        }

        public async Task<ReturnBase<int>> AddPostAsync(Post post)
        {
            try
            {
                var addPostResult = await _postRespository.AddPostAsync(post);

                if (!addPostResult.Succeeded)
                    return Failed<int>(addPostResult.Message);

                return Success(addPostResult.Data, "Post added successfully");
            }
            catch (Exception ex)
            {
                return Failed<int>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdatePostAsync(Post post)
        {
            try
            {
                var updatePostResult = await _postRespository.UpdateAsync(post);

                if (!updatePostResult.Succeeded)
                    return Failed<bool>(updatePostResult.Message);

                return Success(true, "Post updated successfully");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<Post>> GetPostForUpdateAsync(int postId)
        {
            try
            {
                var post = await _postRespository.GetTableNoTracking().Data.Where(x => x.Id == postId).FirstOrDefaultAsync();

                if (post is null)
                    return Failed<Post>("Invalid post id");

                return Success(post);
            }
            catch (Exception ex)
            {
                return Failed<Post>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<Post>> GetPostByIdAsync(int postId)
        {
            try
            {
                var getPostResult = await _postRespository.GetPostAsync(postId);
                if (!getPostResult.Succeeded)
                    return Failed<Post>(getPostResult.Message);

                return Success(getPostResult.Data!);
            }
            catch (Exception ex)
            {
                return Failed<Post>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Post>>> GetPostsAsync()
        {
            try
            {
                var getPostsResult = await _postRespository.GetPostsAsync();

                if (!getPostsResult.Succeeded)
                    return Failed<IQueryable<Post>>(getPostsResult.Message);

                return Success(getPostsResult.Data!);
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<Post>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Post>>> GetUserPostsAsync(string userId)
        {
            try
            {
                var getUserPostsResult = await _postRespository.GetUserPostsAsync(userId);

                if (!getUserPostsResult.Succeeded)
                    return Failed<IQueryable<Post>>(getUserPostsResult.Message);

                return Success(getUserPostsResult.Data!);
            }
            catch (Exception ex)
            {
                return Failed<IQueryable<Post>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
