using AutoMapper;
using Blog.Core.Featuers.Post.Command.Model;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.PostMapping.Command
{
    public class ToggleLikeButtonMappingProfile : Profile
    {
        public ToggleLikeButtonMappingProfile()
        {
            CreateMap<ToggleLikeButtonCommand, Like>();
        }
    }
}
