using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactifyBlog.Data.Extensions;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Data.Data
{
  public class ReactifyBlogDbContext : IdentityDbContext<UserDBO, RoleDBO, long>
  {
    public ReactifyBlogDbContext(DbContextOptions<ReactifyBlogDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshTokenDBO> RefreshTokens  { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<UserDBO>(b =>
      {
        b.Ignore(u => u.PhoneNumber);
        b.Ignore(u => u.PhoneNumberConfirmed);
        b.Ignore(u => u.TwoFactorEnabled);
      });

      builder.SeedRoles();

      builder.ApplyTablePrefix();
    }
  }
}
