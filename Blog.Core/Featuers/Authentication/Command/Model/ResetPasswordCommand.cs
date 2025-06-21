using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class ResetPasswordCommand : IRequest<ReturnBase<bool>>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
