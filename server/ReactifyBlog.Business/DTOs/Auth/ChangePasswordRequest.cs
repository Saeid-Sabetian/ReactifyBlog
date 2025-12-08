namespace ReactifyBlog.Business.DTOs.Auth;

public class ChangePasswordRequest
{
  public required Guid UserId { get; set; }
  public required string OldPassword { get; set; }
  public required string NewPassword { get; set; }
  public required string ConfirmNewPassword { get; set; }
}