using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IConfirmEmailService
    {
        Task<ReturnBase<bool>> SendConfirmationEmailAsync(ApplicationUser user);
        Task<ReturnBase<bool>> ConfirmEmailAsync(string userId, string token);
    }
}
