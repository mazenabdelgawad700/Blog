using AutoMapper;
using Blog.Core.Featuers.ApplicationUserFeatuer.Command.Model;
using Blog.Infrastructure.Abstracts;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.ApplicationUserFeatuer.Command.Handler
{
    public class ApplicationUserCommandHandler : ReturnBaseHandler,
        IRequestHandler<UpdateApplicationUserProfileCommand, ReturnBase<bool>>
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConfirmEmailService _confirmEmailService;

        public ApplicationUserCommandHandler(IApplicationUserService applicationUserService, IMapper mapper, IImageService imageService, IConfirmEmailService confirmEmailService, IApplicationUserRepository applicationUserRepository)
        {
            this._applicationUserService = applicationUserService;
            this._mapper = mapper;
            this._imageService = imageService;
            this._confirmEmailService = confirmEmailService;
            this._applicationUserRepository = applicationUserRepository;
        }


        public async Task<ReturnBase<bool>> Handle(UpdateApplicationUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserByIdResult = await _applicationUserService.GetUserById(request.Id);

                if (!getUserByIdResult.Succeeded)
                    return Failed<bool>("Invalid user id");

                var user = getUserByIdResult.Data!;


                if (request.UserName is not null)
                    user.UserName = request.UserName;

                if (request.Bio is not null)
                    user.Bio = request.Bio;

                if (request.Email is not null)
                {
                    var checkIfEmailAlreadyUsed = await _applicationUserService.IsEmailUsedAsync(request.Email);

                    if (checkIfEmailAlreadyUsed.Succeeded)
                        return Failed<bool>(checkIfEmailAlreadyUsed.Message);

                    user.Email = request.Email;
                    var sendConfirmationEmail = await _confirmEmailService.SendConfirmationEmailAsync(user);

                    while (!sendConfirmationEmail.Succeeded)
                        sendConfirmationEmail = await _confirmEmailService.SendConfirmationEmailAsync(user);
                }

                if (request.ProfilePicture is not null)
                {
                    var deleteImageResult = _imageService.Delete(user.ProfilePictureUrl);
                    var saveNewImageResult = await _imageService.SaveAsync(request.ProfilePicture);

                    if (!saveNewImageResult.Succeeded)
                        return Failed<bool>(saveNewImageResult.Message);

                    user.ProfilePictureUrl = saveNewImageResult.Data!;
                }

                var updateApplicationUserResult = await _applicationUserService.UpdateUserProfileAsync(user);

                if (!updateApplicationUserResult.Succeeded)
                    return Failed<bool>(updateApplicationUserResult.Message);

                return Success(true, updateApplicationUserResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
