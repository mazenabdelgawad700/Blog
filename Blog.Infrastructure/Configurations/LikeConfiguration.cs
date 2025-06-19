using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");

            // Primary Key
            builder.HasKey(l => l.Id);

            // Properties
            builder.Property(l => l.LikedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // Relationships
            builder.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint - prevent duplicate likes
            builder.HasIndex(l => new { l.PostId, l.UserId })
                .IsUnique()
                .HasDatabaseName("IX_Likes_PostId_UserId_Unique");

            // Additional indexes
            builder.HasIndex(l => l.PostId);
            builder.HasIndex(l => l.UserId);
            builder.HasIndex(l => l.LikedAt);
        }
    }
}
