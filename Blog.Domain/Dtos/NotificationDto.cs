using Blog.Domain.Entities;

namespace Blog.Domain.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ActorUserName { get; set; }
        public string ActorUserProfilePicture { get; set; }
        public int? PostId { get; set; }
        public string PostTitle { get; set; }
    }
}
