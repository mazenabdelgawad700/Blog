using Blog.Shared.Base;

namespace Blog.Service.Abstracts
{
    public interface ISendEmailService
    {
        Task<ReturnBase<bool>> SendEmailAsync(string email, string message, string subject, string contentType = "text/plain");
    }
}
