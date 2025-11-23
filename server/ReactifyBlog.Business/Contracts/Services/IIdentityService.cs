using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Contracts.Services
{
	public interface IIdentityService
	{
		Task<AppResponse<AuthResponse>> RegisterUserAsync(RegisterRequest request);
		Task<AppResponse<AuthResponse>> LoginUserAsync(LoginRequest request);
		Task<AppResponse<AuthResponse>> ChangePasswordAsync(string userId, ChangePasswordRequest request);
		Task<AppResponse<AuthResponse>> UpdateUserAsync(string userId, UpdateUserRequest request);
		Task<AppResponse<AuthResponse>> RecoverPasswordAsync(RecoverPasswordRequest request);
		Task<AppResponse<AuthResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
		Task<AppResponse<AuthResponse>> LogoutUserAsync();
		Task<AppResponse<AuthResponse>> RefreshTokenAsync(string refreshToken);
	}
}
