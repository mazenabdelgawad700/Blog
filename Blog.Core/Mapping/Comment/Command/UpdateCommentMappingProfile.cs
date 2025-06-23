using AutoMapper;
using Blog.Core.Featuers.Comment.Command.Model;

namespace Blog.Core.Mapping.Comment.Command
{
    public class UpdateCommentMappingProfile : Profile
    {
        public UpdateCommentMappingProfile()
        {
            CreateMap<UpdateCommentCommand, Domain.Entities.Comment>();
        }
    }
}
