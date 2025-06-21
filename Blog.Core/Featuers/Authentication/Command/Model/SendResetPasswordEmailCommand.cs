using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class SendResetPasswordEmailCommand : IRequest<ReturnBase<bool>>
    {
        public string Email { get; set; } = null!;
    }
}
