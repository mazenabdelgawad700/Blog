using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string? UserRefreshToken { get; set; }
        [Required]
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
