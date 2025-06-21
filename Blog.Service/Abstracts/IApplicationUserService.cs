using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IApplicationUserService
    {
        Task<ReturnBase<bool>> UpdateUserProfileAsync(ApplicationUser user);
        Task<ReturnBase<bool>> IsEmailUsedAsync(string emailAddress);
        Task<ReturnBase<ApplicationUser>> GetUserById(string userId);
    }
}
