using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class ChangePasswordCommand : IRequest<ReturnBase<bool>>
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
