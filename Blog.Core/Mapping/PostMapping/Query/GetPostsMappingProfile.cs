using AutoMapper;
using Blog.Core.Featuers.Post.Query.Response;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.PostMapping.Query
{
    public class GetPostsMappingProfile : Profile
    {
        public GetPostsMappingProfile()
        {
            CreateMap<Post, GetPostsResponse>()
                .ForMember(dest => dest.MainImageUrl,
                    opt => opt.MapFrom(x => x.PostPictures.OrderBy(p => p.DisplayOrder).FirstOrDefault().PictureUrl));
        }
    }
}
