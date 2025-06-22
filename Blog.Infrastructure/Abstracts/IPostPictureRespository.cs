using Blog.Domain.Entities;
using Blog.Domain.IBaseRepository;
using Blog.Shared.Base;

namespace Blog.Infrastructure.Abstracts
{
    public interface IPostPictureRespository : IBaseRepository<PostPicture>
    {
        Task<ReturnBase<bool>> DeleteImageByNameAsync(string imageName);
    }
}
