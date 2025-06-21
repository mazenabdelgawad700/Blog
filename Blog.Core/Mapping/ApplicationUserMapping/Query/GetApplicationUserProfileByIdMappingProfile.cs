using AutoMapper;
using Blog.Core.Featuers.ApplicationUserFeatuer.Query.Response;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.ApplicationUserMapping.Query
{
    public class GetApplicationUserProfileByIdMappingProfile : Profile
    {
        public GetApplicationUserProfileByIdMappingProfile()
        {
            CreateMap<ApplicationUser, GetApplicationUserProfileByIdResponse>();
        }
    }
}
