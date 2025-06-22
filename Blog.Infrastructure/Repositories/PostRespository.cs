using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    internal class PostRespository : BaseRepository<Post>, IPostRespository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Post> _posts;

        public PostRespository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _posts = _dbContext.Set<Post>();
        }

        public async Task<ReturnBase<int>> AddPostAsync(Post post)
        {
            try
            {
                var result = await _posts.AddAsync(post);
                await _dbContext.SaveChangesAsync();
                return Success(result.Entity.Id);
            }
            catch (Exception ex)
            {
                return Failed<int>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<Post>> GetPostAsync(int postId)
        {
            try
            {
                var result = await _posts
                    .Where(x => x.Id == postId && !x.IsDeleted)
                    .Include(x => x.User)
                    .Include(x => x.PostPictures)
                    .Include(x => x.Likes)
                        .ThenInclude(l => l.User)
                    .Include(x => x.Comments.Where(c => !c.IsDeleted && c.IsApproved))
                        .ThenInclude(c => c.User)
                    .Include(x => x.Comments)
                        .ThenInclude(c => c.Replies.Where(r => !r.IsDeleted && r.IsApproved))
                            .ThenInclude(r => r.User)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync();

                if (result is null)
                    return Failed<Post>("Post no longer exists or has been deleted");

                result.ViewsCount++;
                await _dbContext.SaveChangesAsync();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Failed<Post>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<Post>>> GetPostsAsync()
        {
            var posts = _posts
                .Include(x => x.User)
                .Include(x => x.PostPictures)
                .Include(x => x.Likes)
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            return await Task.FromResult(Success(posts));
        }
        public async Task<ReturnBase<IQueryable<Post>>> GetUserPostsAsync(string userId)
        {
            var posts = _posts
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .Include(x => x.PostPictures)
                .Include(x => x.Likes)
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            return await Task.FromResult(Success(posts));
        }
    }
}
