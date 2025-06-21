using Blog.Domain.Entities;
using Blog.Domain.IBaseRepository;

namespace Blog.Infrastructure.Abstracts
{
    public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
    {
    }
}
