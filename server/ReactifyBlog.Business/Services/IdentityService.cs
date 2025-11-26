using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.Exceptions;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;
using System.Security.Cryptography;

namespace ReactifyBlog.Business.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<UserDBO> _userManager;
		private readonly SignInManager<UserDBO> _signInManager;
		private readonly IEmailService _emailService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ReactifyBlogDbContext _dbContext;
		private readonly IMapper _mapper;

		public IdentityService(
		UserManager<UserDBO> userManager,
		SignInManager<UserDBO> signInManager,
		IEmailService emailService,
		IMapper mapper,
		IHttpContextAccessor httpContextAccessor,
		ReactifyBlogDbContext dbContext
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailService = emailService;
			_httpContextAccessor = httpContextAccessor;
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<bool> RegisterUserAsync(RegisterRequest request)
		{
			var user = _mapper.Map<UserDBO>(request);

			var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded)
			{
				string sevenDigitCode = GenerateSecureSevenDigitCode();
				user.ConfirmEmailCode = sevenDigitCode;
				user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);

				await _userManager.UpdateAsync(user);

				if (user.Email != null)
				{
					await _emailService.SendEmailAsync(user.Email, EmailConstants.ConfirmEmailSubject, string.Format(EmailConstants.ConfirmEmailMessage, sevenDigitCode));
				}

				await _userManager.AddToRoleAsync(user, ReactifyBlog.Data.Constants.RoleConstants.UserRole);

				return true;
			}

			return false;
		}

		public async Task<bool> LoginUserAsync(LoginRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				throw new ReactifyBlogException(AuthServiceErrorConstants.InvalidCredentialsErrorCode, nameof(IdentityService), (int)System.Net.HttpStatusCode.BadRequest, AuthServiceErrorConstants.InvalidCredentialsErrorMessage);
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);

				if (request.RememberMe)
				{
					await GenerateAndStoreRefreshTokenAsync(user);
				}

				return true;
			}

			throw new ReactifyBlogException(AuthServiceErrorConstants.InvalidCredentialsErrorCode, nameof(IdentityService), (int)System.Net.HttpStatusCode.BadRequest, AuthServiceErrorConstants.InvalidCredentialsErrorMessage);

		}

		private async Task GenerateAndStoreRefreshTokenAsync(UserDBO user)
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			var refreshToken = Convert.ToBase64String(randomNumber);

			var hashedRefreshToken = _userManager.PasswordHasher.HashPassword(user, refreshToken);

			var refreshTokenDBO = new RefreshTokenDBO
			{
				Id = Guid.NewGuid(),
				TokenHash = hashedRefreshToken,
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedAt = DateTime.UtcNow
			};

			await _dbContext.RefreshTokens.AddAsync(refreshTokenDBO);
			await _dbContext.SaveChangesAsync();

			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = true, // only send on HTTPS
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(7)
			};

			_httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieConstants.RefreshTokenCookieName, refreshToken, cookieOptions);
		}

		public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null)
			{
				throw new ReactifyBlogException(AuthServiceErrorConstants.ChangePasswordFailedErrorCode, nameof(IdentityService), (int)System.Net.HttpStatusCode.NotFound, AuthServiceErrorConstants.ChangePasswordFailedErrorMessage);
			}

			var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
			return result.Succeeded;
		}

		public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null)
			{
				return false;
			}

			user.NickName = request.NickName;
			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded;
		}

		public async Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				return false;
			}

			string sevenDigitCode = GenerateSecureSevenDigitCode();
			user.ConfirmEmailCode = sevenDigitCode;
			user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);
			await _userManager.UpdateAsync(user); // Persist the code and expiration

			if (user.Email != null)
			{
				await _emailService.SendEmailAsync(user.Email, EmailConstants.RecoverPasswordSubject, $"Your password recovery code is: {sevenDigitCode}");
			}
			return true;
		}

		public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null || user.ConfirmEmailCode == null || user.ConfirmEmailExpiration == null || user.ConfirmEmailExpiration < DateTimeOffset.UtcNow || user.ConfirmEmailCode != request.Token)
			{
				return false;
			}

			user.EmailConfirmed = true;
			user.ConfirmEmailCode = null;
			user.ConfirmEmailExpiration = null;
			await _userManager.UpdateAsync(user);

			return true;
		}

		public async Task<bool> LogoutUserAsync()
		{
			await _signInManager.SignOutAsync();
			return true;
		}

		public async Task RefreshTokenAsync(string refreshToken)
		{
			// In a real application, you would validate the refresh token against a stored token (e.g., in a database).
			// If valid, you would revoke the old refresh token, generate new authentication and refresh tokens,
			// and issue a new authentication cookie.
			// For this example, we'll just re-sign in the user if the refresh token is considered valid (e.g., by checking a mock store).
			// This part requires a proper refresh token storage and validation mechanism.
			await Task.CompletedTask;
			throw new NotImplementedException("Refresh token logic needs to be implemented with proper token storage and validation.");
		}

		private string GenerateSecureSevenDigitCode()
		{
			byte[] randomNumber = new byte[4]; // 4 bytes can represent a number up to 2^32 - 1
			RandomNumberGenerator.Fill(randomNumber);
			int code = BitConverter.ToInt32(randomNumber, 0) % 10000000; // Limit to 7 digits
			if (code < 1000000) // Ensure it's always 7 digits (e.g., 0-padded if necessary)
			{
				code += 1000000; // Add 1,000,000 to ensure it's a 7-digit number
			}
			return code.ToString("D7"); // Format to ensure leading zeros if needed
		}
	}
}
