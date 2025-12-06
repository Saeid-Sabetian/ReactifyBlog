using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Contracts.Services
{
	public interface IIdentityService
	{
		Task<bool> RegisterUserAsync(RegisterRequest request);
		Task<bool> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
		Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken);
		Task<bool> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);
		Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request, CancellationToken cancellationToken);
		Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken);
		Task<bool> LogoutUserAsync();
		Task RefreshTokenAsync(string refreshToken);
	}
}
