using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Contracts.Services
{
	public interface IIdentityService
	{
		Task<bool> RegisterUserAsync(RegisterRequest request);
		Task<bool> LoginUserAsync(LoginRequest request);
		Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
		Task<bool> UpdateUserAsync(UpdateUserRequest request);
		Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request);
		Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request);
		Task<bool> LogoutUserAsync();
		Task RefreshTokenAsync(string refreshToken);
	}
}
