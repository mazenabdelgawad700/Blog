using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.RepositoriesBase;

namespace Blog.Infrastructure.Repositories
{
    public class AuthenticationRepository : BaseRepository<ApplicationUser>, IAuthenticationRepository
    {
        public AuthenticationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
