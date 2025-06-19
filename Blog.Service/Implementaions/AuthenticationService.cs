using Blog.Domain.Entities;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Identity;

namespace Blog.Service.Implementaions
{
    internal class AuthenticationService : ReturnBaseHandler, IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfirmEmailService _confirmEmailService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfirmEmailService confirmEmailService)
        {
            this._userManager = userManager;
            _confirmEmailService = confirmEmailService;
        }

        public async Task<ReturnBase<bool>> RegisterUserAsync(ApplicationUser user, string password)
        {
            try
            {
                if (user is null)
                    return Failed<bool>("Invalid user data");

                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Bio) || string.IsNullOrEmpty(user.UserName))
                    return Failed<bool>("Please, enter required fields");

                var isUserExist = await _userManager.FindByEmailAsync(user.Email);

                if (isUserExist is not null)
                    return Failed<bool>("Email Address already used");

                user.Id = Guid.NewGuid().ToString();

                var createUserResult = await _userManager.CreateAsync(user, password);

                if (createUserResult.Succeeded)
                {
                    var sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);

                    while (!sendConfirmationEmailResult.Succeeded)
                        sendConfirmationEmailResult = await _confirmEmailService.SendConfirmationEmailAsync(user);

                    if (user.Email == "mazenabdelgawad700@gmail.com")
                        await _userManager.AddToRoleAsync(user, "Admin");

                    else
                        await _userManager.AddToRoleAsync(user, "User");

                    return Success(true, "user registerd successfully, please confirm your email address");
                }



                return Failed<bool>("Can not register user, pleas try again");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
