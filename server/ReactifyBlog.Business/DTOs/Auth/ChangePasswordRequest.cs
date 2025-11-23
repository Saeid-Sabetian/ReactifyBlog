namespace ReactifyBlog.Business.DTOs.Auth;

public class ChangePasswordRequest
{
	public string CurentPassword { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}

public class ConfirmEmailRequest
{

}