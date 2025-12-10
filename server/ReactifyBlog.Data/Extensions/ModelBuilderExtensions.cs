using Microsoft.EntityFrameworkCore;
using ReactifyBlog.Data.Constants;
using ReactifyBlog.Data.Models;
using ReactifyBlog.Data.Configurations;

namespace ReactifyBlog.Data.Extensions
{
  public static class ModelBuilderExtensions
  {
    public static void SeedRoles(this ModelBuilder builder)
    {
      builder.Entity<RoleDBO>().HasData(
          new RoleDBO { Id = 1, Name = RoleConstants.AdminRole, NormalizedName = RoleConstants.AdminRole.ToUpper(), ConcurrencyStamp = Guid.Parse("d7d32ed6-026a-4e66-98dc-2a6e5266e02a").ToString() },
          new RoleDBO { Id = 2, Name = RoleConstants.UserRole, NormalizedName = RoleConstants.UserRole.ToUpper(), ConcurrencyStamp = Guid.Parse("8737b855-72fe-4ffe-a85b-295c5a998dea").ToString() }
      );
    }

    public static void ApplyTablePrefix(this ModelBuilder builder)
    {
      foreach (var entityType in builder.Model.GetEntityTypes())
      {
        string? tableName = entityType.GetTableName();

        if (tableName != null
            && !entityType.IsOwned()
            && (entityType.BaseType == null || !entityType.BaseType.IsAbstract())
            && !tableName.StartsWith("tbl"))
        {
          entityType.SetTableName("tbl" + tableName);
        }
      }
    }

    public static void ApplyConfigurations(this ModelBuilder builder)
    {
      builder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
  }
}
