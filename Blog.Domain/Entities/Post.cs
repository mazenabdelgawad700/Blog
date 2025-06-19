using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)] // Reasonable limit for blog posts
        public string Title { get; set; }

        [Required]
        public string Content { get; set; } // Changed from Text to Content for clarity

        [StringLength(300)]
        public string? Summary { get; set; } // For previews

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int ViewsCount { get; set; } = 0; // Track post views

        public bool IsDeleted { get; set; } = false; // Soft delete

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedAt { get; set; }

        // Foreign Key
        [Required]
        public string UserId { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<PostPicture> PostPictures { get; set; } = new List<PostPicture>();
    }
}
