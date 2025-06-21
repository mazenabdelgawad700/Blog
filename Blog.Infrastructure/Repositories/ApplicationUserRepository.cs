using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;

namespace Blog.Infrastructure.Repositories
{
    internal class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
