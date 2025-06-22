using Blog.Domain.Entities;
using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface IAuthenticationService
    {
        Task<ReturnBase<bool>> RegisterUserAsync(ApplicationUser user, string password);
        Task<ReturnBase<string>> LoginAsync(string email, string password);
        Task<ReturnBase<bool>> ResetPasswordAsync(string resetPasswordToken, string newPassword, string email);
        Task<ReturnBase<bool>> SendResetPasswordEmailAsync(string email);
        Task<ReturnBase<string>> RefreshTokenAsync(string accessToken);
        Task<ReturnBase<bool>> ChangePasswordAsync(string newPassword, string currentPassword, string userId);
    }
}
