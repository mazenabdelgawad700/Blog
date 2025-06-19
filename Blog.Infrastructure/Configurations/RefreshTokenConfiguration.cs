using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            // Primary Key
            builder.HasKey(rt => rt.Id);

            // Properties
            builder.Property(rt => rt.UserRefreshToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.JwtId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(rt => rt.IsUsed)
                .HasDefaultValue(false);

            builder.Property(rt => rt.IsRevoked)
                .HasDefaultValue(false);

            builder.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // Relationships
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(rt => rt.UserRefreshToken)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token_Unique");

            builder.HasIndex(rt => rt.JwtId);
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.ExpiresAt);
            builder.HasIndex(rt => new { rt.IsUsed, rt.IsRevoked, rt.ExpiresAt });
        }
    }
}
