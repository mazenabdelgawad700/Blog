using Blog.Shared.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Featuers.Authentication.Command.Model
{
    public class RegisterUserCommand : IRequest<ReturnBase<bool>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
