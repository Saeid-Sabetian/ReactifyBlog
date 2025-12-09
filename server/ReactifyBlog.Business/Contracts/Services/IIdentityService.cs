using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Business.Contracts.Services
{
	public interface IIdentityService
	{
		Task RegisterUserAsync(RegisterRequest request);
		Task LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
		Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken);
		Task<bool> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);
		Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request, CancellationToken cancellationToken);
		Task ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken);
		Task LogoutUserAsync();
		Task GenerateAndStoreRefreshTokenAsync(UserDBO user, CancellationToken cancellationToken);
	}
}
