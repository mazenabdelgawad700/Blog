using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // User who will receive the notification

        [Required]
        public string ActorUserId { get; set; } // User who performed the action

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public int? PostId { get; set; } // Related post (nullable for general notifications)
        public int? CommentId { get; set; } // Related comment (nullable)

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } // Recipient
        public virtual ApplicationUser ActorUser { get; set; } // Who performed the action
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
    }

    public enum NotificationType
    {
        Comment = 1,
        Like = 2,
        NewPost = 3,
        Reply = 4
    }
}
