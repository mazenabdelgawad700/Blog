using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;

namespace Blog.Infrastructure.Repositories
{
    internal class PostPictureRespository : BaseRepository<PostPicture>, IPostPictureRespository
    {
        public PostPictureRespository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
