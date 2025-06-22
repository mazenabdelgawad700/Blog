using Blog.Shared.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Featuers.Post.Command.Model
{
    public class UpdatePostCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<string>? OldImages { get; set; }
    }
}
/*
 
 post (title, content, 3 images)
 
1 - replace image(s) (image(s) name, new image)
2 - add new image(s) (new images)
3 - delete image (image name)


 */