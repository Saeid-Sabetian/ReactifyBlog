using ReactifyBlog.Data.Data;
using ReactifyBlog.UnitTestToolkit.Seed;

namespace ReactifyBlog.UnitTestToolkit;

public class TestDataSeeder
{
  private readonly ReactifyBlogDbContext _dbContext;

  public TestDataSeeder(ReactifyBlogDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task SeedAllAsync()
  {
    await UserSeed.Seed(_dbContext);
  }
}
