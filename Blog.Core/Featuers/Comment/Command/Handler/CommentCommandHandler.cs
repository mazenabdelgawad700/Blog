using AutoMapper;
using Blog.Core.Featuers.Comment.Command.Model;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Command.Handler
{
    public class CommentCommandHandler : ReturnBaseHandler,
        IRequestHandler<AddCommentCommand, ReturnBase<bool>>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentCommandHandler(ICommentService commentService, IMapper mapper)
        {
            this._commentService = commentService;
            this._mapper = mapper;
        }
        public async Task<ReturnBase<bool>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mappedResult = _mapper.Map<Domain.Entities.Comment>(request);
                var addCommentResult = await _commentService.AddCommentAsync(mappedResult);

                if (!addCommentResult.Succeeded)
                    return Failed<bool>(addCommentResult.Message);

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
