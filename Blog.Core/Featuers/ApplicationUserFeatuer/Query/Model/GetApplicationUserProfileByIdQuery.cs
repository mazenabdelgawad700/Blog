using Blog.Core.Featuers.ApplicationUserFeatuer.Query.Response;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.ApplicationUserFeatuer.Query.Model
{
    public class GetApplicationUserProfileByIdQuery : IRequest<ReturnBase<GetApplicationUserProfileByIdResponse>>
    {
        public string UserId { get; set; }
    }
}
