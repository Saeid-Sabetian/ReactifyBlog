using Microsoft.EntityFrameworkCore;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.UnitTestToolkit.Seed;

public static class UserSeed
{
  public static async Task Seed(ReactifyBlogDbContext db)
  {
    if (db.Users.Any()) return;

    await db.Users.AddRangeAsync(GetUserDBOList());

    await db.SaveChangesAsync();
  }

  private static List<UserDBO> GetUserDBOList()
  {
    return new List<UserDBO>
    {
     new UserDBO
         {
           Id = 1,
           Email = "existing@test.com",
           UserName = "existing@test.com",
           ConfirmEmailCode = null,
           ConfirmEmailExpiration = null,
           EmailConfirmed = true,
           LockoutEnabled = true,
           NormalizedEmail = "EXISTING@TEST.COM",
           NormalizedUserName = "EXISTING@TEST.COM",
           NickName = "test"
         },
    };
  }
}
