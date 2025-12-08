namespace ReactifyBlog.Business.DTOs.Auth;

public class ConfirmEmailRequest
{
  public string Email { get; set; }
  public string ConfirmationCode { get; set; }
}