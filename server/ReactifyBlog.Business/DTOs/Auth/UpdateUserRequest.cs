namespace ReactifyBlog.Business.DTOs.Auth;

public class UpdateUserRequest
{
    public required Guid UserId { get; set; }
    public required string NickName { get; set; }
    public required string Email { get; set; }
}