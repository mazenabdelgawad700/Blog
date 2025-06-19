using Blog.Domain.Entities;
using Blog.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPicture> PostPictures { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostPictureConfiguration());
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            // Additional global configurations
            ConfigureDecimalPrecision(modelBuilder);
            ConfigureDeleteBehavior(modelBuilder);
        }
        private static void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
        {
            // Configure decimal precision if you add any decimal properties later
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
        private static void ConfigureDeleteBehavior(ModelBuilder modelBuilder)
        {
            // Set default delete behavior to Restrict to prevent accidental cascading deletes
            // We've already configured specific cascade behaviors in the entity configurations above
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                if (relationship.DeleteBehavior == DeleteBehavior.Cascade)
                {
                    // Keep the cascade deletes we explicitly configured
                    continue;
                }
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically update UpdatedAt timestamps
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Post post)
                {
                    post.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Comment comment)
                {
                    comment.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}