using AutoMapper;
using Blog.Core.Featuers.Authentication.Command.Model;
using Blog.Domain.Entities;

namespace Blog.Core.Mapping.AuthenticationMapping.Command
{
    public class RegisterUserMappingProfile : Profile
    {
        public RegisterUserMappingProfile()
        {
            CreateMap<RegisterUserCommand, ApplicationUser>()
                .ForMember(src => src.ProfilePictureUrl, opt => opt.Ignore());
        }
    }
}
