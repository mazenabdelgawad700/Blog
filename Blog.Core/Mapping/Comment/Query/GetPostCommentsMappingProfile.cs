using AutoMapper;
using Blog.Core.Featuers.Comment.Query.Response;

namespace Blog.Core.Mapping.Comment.Query
{
    public class GetPostCommentsMappingProfile : Profile
    {
        public GetPostCommentsMappingProfile()
        {
            CreateMap<Domain.Entities.Comment, GetPostCommentsResponse>()
                .ForMember(dest => dest.RepliesCount, opt => opt.MapFrom(src =>
                    src.Replies.Where(r => !r.IsDeleted).Count()));
        }
    }
}
