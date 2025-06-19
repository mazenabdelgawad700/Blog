using System.Net;

namespace Blog.Shared.Base
{
    public class ReturnBase<T>
    {
        public ReturnBase(string? message = null)
        {
            Succeeded = true;
            Errors = [];
            Message = message ?? "";
        }
        public ReturnBase(T data, string? message = null)
        {
            Succeeded = true;
            Message = message!;
            Data = data;
            Errors = [];
        }
        public ReturnBase(T data, string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
            Errors = [];
            Data = data;
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T? Data { get; set; }
    }
}
