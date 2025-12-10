using Microsoft.AspNetCore.Identity;

namespace ReactifyBlog.Data.Models
{
  public class UserDBO : IdentityUser<long>
  {
    public string NickName { get; set; }
    public string? ConfirmEmailCode { get; set; }
    public DateTimeOffset? ConfirmEmailExpiration { get; set; }
    public bool IsPasswordRecoveryRequested { get; set; }
    public string? PasswordRecoveryCode { get; set; }
    public DateTime? PasswordRecoveryExpiresAt { get; set; }

    //Navigation Properties
    public RefreshTokenDBO? RefreshToken { get; set; }
  }
}