using AutoMapper;
using Blog.Core.Featuers.Post.Command.Model;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.PostMapping.Command
{
    public class AddPostMappingProfile : Profile
    {
        public AddPostMappingProfile()
        {
            CreateMap<AddPostCommand, Post>();
            CreateMap<AddPostCommand, PostPicture>();
        }
    }
}
