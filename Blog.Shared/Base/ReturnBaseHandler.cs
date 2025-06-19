using System.Net;

namespace Blog.Shared.Base
{
    public class ReturnBaseHandler
    {
        public ReturnBase<T> Success<T>(T entity, string? message = null)
        {
            return new ReturnBase<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Success",
            };
        }
        public ReturnBase<T> Failed<T>(string? message = null)
        {
            return new ReturnBase<T>()
            {
                StatusCode = HttpStatusCode.ExpectationFailed,
                Succeeded = false,
                Message = message ?? "Failed"
            };
        }
    }
}
