using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            // Primary Key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode();

            builder.Property(p => p.Content)
                .IsRequired()
                .IsUnicode();

            builder.Property(p => p.Summary)
                .HasMaxLength(300)
                .IsUnicode();

            builder.Property(p => p.LikesCount)
                .HasDefaultValue(0);

            builder.Property(p => p.CommentsCount)
                .HasDefaultValue(0);

            builder.Property(p => p.ViewsCount)
                .HasDefaultValue(0);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            // Relationships
            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.CreatedAt);
            builder.HasIndex(p => p.IsDeleted);
            builder.HasIndex(p => new { p.IsDeleted, p.CreatedAt });
        }
    }
}
