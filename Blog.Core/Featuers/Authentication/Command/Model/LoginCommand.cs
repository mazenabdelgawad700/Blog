using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class LoginCommand : IRequest<ReturnBase<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
