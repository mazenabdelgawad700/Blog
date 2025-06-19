using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IAuthenticationService
    {
        Task<ReturnBase<bool>> RegisterUserAsync(ApplicationUser user, string password);
    }
}
