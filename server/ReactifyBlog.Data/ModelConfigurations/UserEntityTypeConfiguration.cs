using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Data.ModelConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDBO>
{
	public void Configure(EntityTypeBuilder<UserDBO> builder)
	{
		builder.HasKey(p => p.Id);
	}
}
