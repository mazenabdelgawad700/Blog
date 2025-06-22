using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;

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
    }
}
