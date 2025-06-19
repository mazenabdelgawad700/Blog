using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class PostPicture
    {
        public int Id { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public int DisplayOrder { get; set; } = 0; // For ordering multiple pictures
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int PostId { get; set; }
        // Navigation property
        public virtual Post Post { get; set; }
    }
}
