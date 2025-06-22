using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;
using Blog.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    internal class PostPictureRespository : BaseRepository<PostPicture>, IPostPictureRespository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<PostPicture> _postPictures;
        public PostPictureRespository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _postPictures = dbContext.Set<PostPicture>();
        }
        public async Task<ReturnBase<bool>> DeleteImageByNameAsync(string imageName)
        {
            try
            {
                var entity = await _postPictures.Where(x => x.PictureUrl == imageName).FirstOrDefaultAsync();

                if (entity is null)
                    return Failed<bool>($"image {imageName} not found");

                var removeEntityResult = _postPictures.Remove(entity);
                await _dbContext.SaveChangesAsync();

                if (removeEntityResult is null)
                    return Failed<bool>("image can not be removed");

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
