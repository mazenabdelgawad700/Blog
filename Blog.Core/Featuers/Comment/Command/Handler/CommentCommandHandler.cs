using AutoMapper;
using Blog.Core.Featuers.Comment.Command.Model;
using Blog.Service.Abstracts;
using Blog.Shared.Base;
using MediatR;

namespace Blog.Core.Featuers.Comment.Command.Handler
{
    public class CommentCommandHandler : ReturnBaseHandler,
        IRequestHandler<AddCommentCommand, ReturnBase<bool>>,
        IRequestHandler<UpdateCommentCommand, ReturnBase<bool>>,
        IRequestHandler<DeleteCommentCommand, ReturnBase<bool>>
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
        public async Task<ReturnBase<bool>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var commentResult = await _commentService.GetCommentForUpdateAsync(request.Id);

                if (!commentResult.Succeeded)
                    return Failed<bool>(commentResult.Message);

                if (!string.IsNullOrEmpty(request.Content))
                    commentResult.Data.Content = request.Content;

                var updateResult = await _commentService.UpdateCommentAsync(commentResult.Data);
                if (!updateResult.Succeeded)
                    return Failed<bool>(updateResult.Message);

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteCommentResult = await _commentService.DeleteCommentRepliesAsync(request.Id, request.UserId);

                if (!deleteCommentResult.Succeeded)
                    return Failed<bool>(deleteCommentResult.Message);

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
