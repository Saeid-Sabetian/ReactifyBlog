using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenDBO>
{
	public void Configure(EntityTypeBuilder<RefreshTokenDBO> builder)
	{
		builder.HasKey(rt => rt.Id);

		builder.Property(rt => rt.TokenHash)
			.IsRequired()
			.HasMaxLength(256);

		builder.Property(rt => rt.ExpiresAt)
			.IsRequired();

		builder.Property(rt => rt.CreatedAt)
			.IsRequired()
			.HasDefaultValueSql("GETUTCDATE()");

		builder.HasOne(rt => rt.User)
			.WithMany()
			.HasForeignKey(rt => rt.UserId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}
}
