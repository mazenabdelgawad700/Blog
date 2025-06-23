using AutoMapper;
using Blog.Core.Featuers.Comment.Query.Response;

namespace Blog.Core.Mapping.Comment.Query
{
    public class GetCommentRepliesMappingProfile : Profile
    {
        public GetCommentRepliesMappingProfile()
        {
            CreateMap<Domain.Entities.Comment, GetCommentRepliesResponse>();
        }
    }
}
