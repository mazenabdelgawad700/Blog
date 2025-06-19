using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } // Changed from Text to Content

        public bool IsDeleted { get; set; } = false; // Soft delete
        public bool IsApproved { get; set; } = true; // For moderation

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        [Required]
        public string UserId { get; set; }

        public int PostId { get; set; }

        // Self-referencing for replies (optional feature)
        public int? ParentCommentId { get; set; }

        // Navigation properties
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
