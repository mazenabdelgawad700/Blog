using AutoMapper;
using Blog.Core.Featuers.Post.Query.Response;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.PostMapping.Query
{
    public class GetUserPostsMappingProfile : Profile
    {
        public GetUserPostsMappingProfile()
        {
            CreateMap<Post, GetUserPostsResponse>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MainImageUrl,
                    opt => opt.MapFrom(x => x.PostPictures.OrderBy(p => p.DisplayOrder).FirstOrDefault().PictureUrl));
        }
    }
}
