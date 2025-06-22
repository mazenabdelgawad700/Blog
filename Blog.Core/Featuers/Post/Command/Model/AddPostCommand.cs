using Blog.Shared.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Featuers.Post.Command.Model
{
    public class AddPostCommand : IRequest<ReturnBase<bool>>
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string UserId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
