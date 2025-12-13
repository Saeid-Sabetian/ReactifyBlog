namespace ReactifyBlog.Data.Models;

public class RefreshTokenDBO
{
	public long Id { get; set; }
	public string TokenHash { get; set; }
	public DateTime ExpiresAt { get; set; }
	public DateTime CreatedAt { get; set; }
	public long UserId { get; set; }

	//Navigation Properties
	public UserDBO? User { get; set; }
}
