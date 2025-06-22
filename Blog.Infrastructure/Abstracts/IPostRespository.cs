using Blog.Domain.Entities;
using Blog.Domain.IBaseRepository;
using Blog.Shared.Base;

namespace Blog.Infrastructure.Abstracts
{
    public interface IPostRespository : IBaseRepository<Post>
    {
        Task<ReturnBase<int>> AddPostAsync(Post post);
        Task<ReturnBase<Post>> GetPostAsync(int postId);
        Task<ReturnBase<IQueryable<Post>>> GetPostsAsync();
    }
}
