using AutoMapper;
using Blog.Core.Featuers.ApplicationUserFeatuer.Command.Model;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.ApplicationUserMapping.Command
{
    public class UpdateApplicationUserMappingProfile : Profile
    {
        public UpdateApplicationUserMappingProfile()
        {
            CreateMap<UpdateApplicationUserProfileCommand, ApplicationUser>();
        }
    }
}
