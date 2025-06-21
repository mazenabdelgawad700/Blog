using AutoMapper;
using Blog.Core.Featuers.Authentication.Command.Model;
using Blog.Domain.Entities;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Hnadler
{
    public class AuthenticationCommandHandler : ReturnBaseHandler, IRequestHandler<RegisterUserCommand, ReturnBase<bool>>,
        IRequestHandler<ConfirmEmailCommand, ReturnBase<bool>>,
        IRequestHandler<ResetPasswordCommand, ReturnBase<bool>>,
        IRequestHandler<SendResetPasswordEmailCommand, ReturnBase<bool>>,
        IRequestHandler<LoginCommand, ReturnBase<string>>,
        IRequestHandler<RefreshTokenCommand, ReturnBase<string>>
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IImageService _imageService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly IMapper _mapper;

        public AuthenticationCommandHandler(IAuthenticationService authenticationService, IMapper mapper, IImageService imageService, IConfirmEmailService confirmEmailService)
        {
            this._authenticationService = authenticationService;
            this._mapper = mapper;
            _imageService = imageService;
            _confirmEmailService = confirmEmailService;
        }

        public async Task<ReturnBase<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var saveImageResult = await _imageService.SaveAsync(request.ProfilePicture);

                if (!saveImageResult.Succeeded)
                    return Failed<bool>("Failed to register user, please try again");

                var mappedResult = _mapper.Map<ApplicationUser>(request);

                if (mappedResult is null)
                    return Failed<bool>("Failed to register user");

                mappedResult.ProfilePictureUrl = saveImageResult.Data;

                var registerUserResult = await _authenticationService.RegisterUserAsync(mappedResult, request.Password);

                if (!registerUserResult.Succeeded)
                    return Failed<bool>(registerUserResult.Message);

                return Success(true, registerUserResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> confrimEmailResult = await _confirmEmailService.ConfirmEmailAsync(request.UserId, request.Token);

                if (confrimEmailResult.Succeeded)
                {
                    return Success(true, confrimEmailResult.Message);
                }
                return Failed<bool>(confrimEmailResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var loginResult = await _authenticationService.LoginAsync(request.Email, request.Password);

                if (!loginResult.Succeeded)
                    return Failed<string>(loginResult.Message);

                return Success(loginResult.Data!, loginResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<string>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resetPasswordResult = await _authenticationService.ResetPasswordAsync(request.ResetPasswordToken, request.NewPassword, request.Email);

                if (!resetPasswordResult.Succeeded)
                    return Failed<bool>(resetPasswordResult.Message);

                return Success(true, resetPasswordResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sendResentPasswordEmailResult = await _authenticationService.SendResetPasswordEmailAsync(request.Email);

                if (!sendResentPasswordEmailResult.Succeeded)
                    return Failed<bool>(sendResentPasswordEmailResult.Message);

                return Success(sendResentPasswordEmailResult.Data, sendResentPasswordEmailResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshTokenResult = await _authenticationService.RefreshTokenAsync(request.AccessToken);

                if (!refreshTokenResult.Succeeded)
                    return Failed<string>(refreshTokenResult.Message);

                return Success(refreshTokenResult.Data!, refreshTokenResult.Message);
            }
            catch (Exception ex)
            {
                return Failed<string>(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
