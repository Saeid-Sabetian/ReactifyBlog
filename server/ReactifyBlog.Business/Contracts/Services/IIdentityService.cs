using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Business.Contracts.Services
{
	public interface IIdentityService
	{
		Task RegisterUserAsync(RegisterRequest request);
		Task LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
		Task LogoutUserAsync();
		Task ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken);
		Task GenerateAndStoreRefreshTokenAsync(UserDBO user, CancellationToken cancellationToken);
	}
}
