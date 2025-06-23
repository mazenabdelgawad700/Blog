using Blog.Domain.Dtos;

namespace Blog.Service.Abstracts
{
    public interface INotificationService
    {
        Task CreateCommentNotificationAsync(int postId, int commentId, string actorUserId);
        Task CreateLikeNotificationAsync(int postId, string actorUserId);
        Task CreateNewPostNotificationAsync(int postId, string authorUserId);
        Task CreateReplyNotificationAsync(int commentId, int replyId, string actorUserId);
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId, int pageSize = 20);
        Task MarkAsReadAsync(int notificationId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
