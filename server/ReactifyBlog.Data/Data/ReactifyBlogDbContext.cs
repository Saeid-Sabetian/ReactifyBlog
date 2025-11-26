using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactifyBlog.Data.Models;
using ReactifyBlog.Data.Extensions;

namespace ReactifyBlog.Data.Data
{
    public class ReactifyBlogDbContext : IdentityDbContext<UserDBO, RoleDBO, Guid>
    {
        public ReactifyBlogDbContext(DbContextOptions<ReactifyBlogDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshTokenDBO> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Remove PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled from UserDBO
            builder.Entity<UserDBO>(b =>
            {
                b.Ignore(u => u.PhoneNumber);
                b.Ignore(u => u.PhoneNumberConfirmed);
                b.Ignore(u => u.TwoFactorEnabled);
            });

            // Seed Roles
            builder.SeedRoles();

            // Apply 'tbl' prefix to all other tables (if any, not explicitly handled above)
            builder.ApplyTablePrefix();
        }
    }
}
