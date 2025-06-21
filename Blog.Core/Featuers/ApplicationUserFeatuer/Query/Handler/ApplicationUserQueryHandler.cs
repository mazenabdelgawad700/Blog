using AutoMapper;
using Blog.Core.Featuers.ApplicationUserFeatuer.Query.Model;
using Blog.Core.Featuers.ApplicationUserFeatuer.Query.Response;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.ApplicationUserFeatuer.Query.Handler
{
    public class ApplicationUserQueryHandler : ReturnBaseHandler, IRequestHandler<GetApplicationUserProfileByIdQuery, ReturnBase<GetApplicationUserProfileByIdResponse>>
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;

        public ApplicationUserQueryHandler(IApplicationUserService applicationUserService, IMapper mapper)
        {
            this._applicationUserService = applicationUserService;
            this._mapper = mapper;
        }

        public async Task<ReturnBase<GetApplicationUserProfileByIdResponse>> Handle(GetApplicationUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserProfileResult = await _applicationUserService.GetUserByIdAsync(request.UserId);

                if (!getUserProfileResult.Succeeded)
                    return Failed<GetApplicationUserProfileByIdResponse>(getUserProfileResult.Message);

                var mappedResult = _mapper.Map<GetApplicationUserProfileByIdResponse>(getUserProfileResult.Data);

                return Success(mappedResult);
            }
            catch (Exception ex)
            {
                return Failed<GetApplicationUserProfileByIdResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
