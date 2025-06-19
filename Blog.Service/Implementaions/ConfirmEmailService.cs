using Blog.Domain.Entities;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Blog.Service.Implementaions
{
    public class ConfirmEmailService : ReturnBaseHandler, IConfirmEmailService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISendEmailService _emailService;

        public ConfirmEmailService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
                    ISendEmailService emailService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<ReturnBase<bool>> SendConfirmationEmailAsync(ApplicationUser user)
        {
            try
            {
                if (user is not null)
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string encodedToken = WebUtility.UrlEncode(code);
                    HttpRequest resquestAccessor = _httpContextAccessor.HttpContext.Request;

                    UriBuilder uriBuilder = new()
                    {
                        Scheme = resquestAccessor.Scheme,
                        Host = resquestAccessor.Host.Host,
                        Port = resquestAccessor.Host.Port ?? -1,
                        Path = "api/authentication/ConfirmEmail",
                        Query = $"userId={user.Id}&token={encodedToken}"
                    };

                    string returnUrl = uriBuilder.ToString();

                    string message = $"To Confirm Email Click Link: <a href=\"{returnUrl}\">Confirmation Link</a>";

                    ReturnBase<bool> sendEmailResult = await _emailService.SendEmailAsync(user.Email, message, "Confirmation Link", "text/html");

                    return Success(sendEmailResult.Data == true, sendEmailResult.Message);
                }
                return Failed<bool>("Can not send confirmation email");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Failed<bool>("Invalid Token");
                }
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                    return Failed<bool>("Invalid User Id");

                IdentityResult confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

                if (confirmEmailResult.Succeeded)
                    return Success(true, "Email confirmed successfully");

                return Failed<bool>("Failed to confirm email address, please try again");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException.Message);
            }
        }
    }
}
