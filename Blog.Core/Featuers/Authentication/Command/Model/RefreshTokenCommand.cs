using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class RefreshTokenCommand : IRequest<ReturnBase<string>>
    {
        public string AccessToken { get; set; } = null!;
    }
}
