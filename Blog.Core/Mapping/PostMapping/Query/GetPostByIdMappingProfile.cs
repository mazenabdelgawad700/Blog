using AutoMapper;
using Blog.Core.Featuers.Post.Query.Response;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.PostMapping.Query
{
    public class GetPostByIdMappingProfile : Profile
    {
        public GetPostByIdMappingProfile()
        {
            CreateMap<Post, GetPostByIdResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName ?? "Unknown"))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.PostPictures.OrderBy(p => p.DisplayOrder)))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src =>
                    src.Comments.Where(c => !c.IsDeleted && c.IsApproved).OrderBy(c => c.CreatedAt)))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore()); // Handle this separately

            // PostPicture mapping
            CreateMap<PostPicture, PostPictureDto>();

            // Like mapping
            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName ?? "Unknown"))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePictureUrl));

            // Comment mapping with recursive replies
            CreateMap<Domain.Entities.Comment, CommentDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.IsDeleted ? "[Comment deleted]" : src.Content))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName ?? "Unknown"))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src =>
                    src.Replies.Where(r => !r.IsDeleted && r.IsApproved)))
                .ForMember(dest => dest.RepliesCount, opt => opt.MapFrom(src =>
                    src.Replies.Count(r => !r.IsDeleted && r.IsApproved)));
        }
    }
}
