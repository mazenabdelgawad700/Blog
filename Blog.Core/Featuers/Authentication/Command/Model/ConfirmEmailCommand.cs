using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class ConfirmEmailCommand : IRequest<ReturnBase<bool>>
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
