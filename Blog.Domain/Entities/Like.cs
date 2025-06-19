using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class Like
    {
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
