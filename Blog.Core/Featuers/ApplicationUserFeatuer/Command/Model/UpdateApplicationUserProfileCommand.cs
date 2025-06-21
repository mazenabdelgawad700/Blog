using Blog.Shared.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Featuers.ApplicationUserFeatuer.Command.Model
{
    public class UpdateApplicationUserProfileCommand : IRequest<ReturnBase<bool>>
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
