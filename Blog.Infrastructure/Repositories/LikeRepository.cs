using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;

namespace Blog.Infrastructure.Repositories
{
    internal class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
