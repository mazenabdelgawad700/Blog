using Blog.Domain.Entities;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Implementaions
{
    internal class ApplicationUserService : ReturnBaseHandler, IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager)
        {
            this._applicationUserRepository = applicationUserRepository;
            this._userManager = userManager;
        }


        public async Task<ReturnBase<bool>> UpdateUserProfileAsync(ApplicationUser user)
        {
            try
            {
                var updateUserResult = await _applicationUserRepository.UpdateAsync(user);

                if (!updateUserResult.Succeeded)
                    return Failed<bool>(updateUserResult.Message);

                return Success(true, updateUserResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsEmailUsedAsync(string emailAddress)
        {
            try
            {
                var getUserByEmailResult = await _userManager.FindByEmailAsync(emailAddress);

                if (getUserByEmailResult is null)
                    return Success(true, "email address already used");

                return Failed<bool>("email address not used before");
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<ApplicationUser>> GetUserByIdAsync(string userId)
        {
            try
            {
                var getUserByIdResult = await _applicationUserRepository?.GetTableNoTracking()?.Data?.Where(x => x.Id == userId).FirstOrDefaultAsync()!;

                if (getUserByIdResult is null)
                    return Failed<ApplicationUser>("Invalid id");

                return Success(getUserByIdResult);
            }
            catch (Exception ex)
            {
                return Failed<ApplicationUser>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
