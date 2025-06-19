using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class PostPictureConfiguration : IEntityTypeConfiguration<PostPicture>
    {
        public void Configure(EntityTypeBuilder<PostPicture> builder)
        {
            builder.ToTable("PostPictures");

            // Primary Key
            builder.HasKey(pp => pp.Id);

            // Properties
            builder.Property(pp => pp.PictureUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pp => pp.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(pp => pp.UploadedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // Relationships
            builder.HasOne(pp => pp.Post)
                .WithMany(p => p.PostPictures)
                .HasForeignKey(pp => pp.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pp => pp.PostId);
            builder.HasIndex(pp => new { pp.PostId, pp.DisplayOrder });
        }
    }
}
