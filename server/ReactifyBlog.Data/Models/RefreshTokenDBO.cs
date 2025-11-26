namespace ReactifyBlog.Data.Models;

public class RefreshTokenDBO
{
	public Guid Id { get; set; }
	public string TokenHash { get; set; }
	public DateTime ExpiresAt { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? RevokedAt { get; set; }
	public Guid UserId { get; set; }

	//Navigation Properties
	public UserDBO? User { get; set; }
}
