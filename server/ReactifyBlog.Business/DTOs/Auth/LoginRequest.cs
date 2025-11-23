namespace ReactifyBlog.Business.DTOs.Auth;

public class LoginRequest
{
	public string Emaill { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}