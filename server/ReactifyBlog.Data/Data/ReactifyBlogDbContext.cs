using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Data.Data
{
    public class ReactifyBlogDbContext : IdentityDbContext<ReactifyBlogUser, ReactifyBlogRole, Guid>
    {
        public ReactifyBlogDbContext(DbContextOptions<ReactifyBlogDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ReactifyBlogUser>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();
        }
    }
}
