using AutoMapper;
using Blog.Core.Featuers.Comment.Command.Model;

namespace Blog.Core.Mapping.Comment.Command
{
    public class AddCommentMappingProfile : Profile
    {
        public AddCommentMappingProfile()
        {
            CreateMap<AddCommentCommand, Domain.Entities.Comment>();
        }
    }
}
